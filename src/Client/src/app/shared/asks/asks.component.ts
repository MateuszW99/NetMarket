import { Component, OnDestroy, OnInit } from '@angular/core';
import { AsksService } from "./asks.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ItemDetails } from "../items/item-details/item-details.model";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import { Subject, Subscription } from "rxjs";
import { SettingsService } from "../../account/settings.service";
import { FeesService } from "../services/fees/fees.service";
import { catchError, debounceTime, distinctUntilChanged, takeUntil } from "rxjs/operators";
import { AuthService } from "../../auth/auth.service";
import { User } from "../../auth/user.model";
import { TransactionService } from "../services/orders/transaction.service";
import { UpdateOrder } from "../../account/user-orders-table/order-edit/update-order";

@Component({
  selector: 'app-asks',
  templateUrl: './asks.component.html',
  styleUrls: ['./asks.component.css']
})
export class AsksComponent implements OnInit, OnDestroy {

  loading = true;
  loadingSubscription: Subscription;
  error = '';
  errorSubscription: Subscription;

  user: User;
  userSubscription: Subscription;
  sellerLevel: string = '';
  sellerLevelSubscription: Subscription;
  userWantsToPlaceAsk: boolean;
  form: FormGroup;
  itemDetails: ItemDetails;
  size: string;
  totalPrice: number = 0;
  fee: number = 0;
  feeSubscription: Subscription;

  label: string;
  formAction = (): void => {};

  private unsubscribe = new Subject();
  private id: string;
  private canUpdate: boolean = false;
  private defaultLabel: string = 'Place Ask';
  private transactionLabel = 'Sell Now';
  private highestBid: number;

  constructor(
    private askService: AsksService,
    private settingsService: SettingsService,
    private authService: AuthService,
    private toastrService: ToastrService,
    private feesService: FeesService,
    private transactionService: TransactionService,
    private router: Router) { }

  ngOnInit(): void {
    if (history.state.data === undefined) {
      this.router.navigate(['..']);
    }

    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;
    let price = undefined;
    if (this.itemDetails.highestBid){
      this.highestBid = +this.itemDetails.highestBid.price;
    }
    else if (history.state.data.id) {
      this.id = history.state.data.id;
      this.canUpdate = true;
      this.defaultLabel = 'Update Ask';
      price = history.state.data.price;
      this.highestBid = +history.state.data.item.highestBid;
    }

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item.id, Validators.nullValidator),
      size: new FormControl(this.size, Validators.nullValidator),
      price: new FormControl(price, [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,4})?$') ]),
    });

    if (this.form.controls['price'].value !== 0 || this.form.controls['price'].value !== undefined) {
      this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.controls['price'].value);
      this.totalPrice = this.fee + this.form.controls['price'].value;
    }

    this.userWantsToPlaceAsk = true;
    this.label = this.defaultLabel;
    this.formAction = this.onPlaceAsk;

    this.userSubscription = this.authService.user
      .subscribe((user: User) => {
        this.user = user;
      }
    );

    if (this.user === null) {
      this.router.navigate(['/auth']);
    }

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item.id, Validators.nullValidator),
      size: new FormControl(this.size, Validators.nullValidator),
      price: new FormControl(price, [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$') ]),
    });

    this.loadingSubscription = this.settingsService.loading
      .pipe(takeUntil(this.unsubscribe))
      .subscribe((isLoading) => {
        this.loading = isLoading;
      }
    );

    this.sellerLevelSubscription = this.settingsService.userSellerLevelChanged
      .pipe(takeUntil(this.unsubscribe))
      .subscribe((sellerLevel) => {
        this.sellerLevel = sellerLevel;
        this.error = '';
      }
    );

    this.errorSubscription = this.settingsService.errorCatched
      .pipe(takeUntil(this.unsubscribe))
      .subscribe((errorMessage) => {
        this.error = errorMessage;
      }
    );

    this.settingsService.getUserSellerLevel();

    this.feeSubscription = this.form.controls['price'].valueChanges.pipe(
      takeUntil(this.unsubscribe),
      debounceTime(1000),
      distinctUntilChanged()
    ).subscribe(() => {
        this.onPriceChange();
    });
  }

  onSubmitForm(): void {
    this.formAction();
  }

  onPlaceAsk(): void {
    if (this.canUpdate) {
      this.askService.updateAsk(new UpdateOrder(this.id, this.form.controls['size'].value, this.form.controls['price'].value.toString()))        .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrService.success('Ask updated!'));
      })
    } else {
      this.askService.placeAsk(this.form.controls['item'].value, this.form.controls['size'].value, this.form.controls['price'].value.toString())
        .subscribe(() => {
          this.router.navigate([`/items/${this.itemDetails.item.id}`])
            .then(() => this.toastrService.success('Ask placed!'));
        });
    }
  }

  onSellNow(): void {
    this.transactionService.sellNow(this.itemDetails.highestBid.id)
      .pipe(
        catchError(err => {
            this.router.navigate([`/items/${this.itemDetails.item.id}`]);
            this.toastrService.clear();
            this.toastrService.error('An error occurred. Try again later.');
            return err;
        })
      )
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`]);
        this.toastrService.clear();
        this.toastrService.success('Congratulations! You just sold an item!');
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private onPriceChange(): void {
    const userPrice = this.form.controls['price'].value;
    if (!this.highestBid) {
      this.userWantsToPlaceAsk = true;
    } else {
      this.userWantsToPlaceAsk = userPrice > this.highestBid;
    }

    if (this.userWantsToPlaceAsk && userPrice !== this.highestBid) {
      this.toastrService.clear();
      this.label = this.defaultLabel;
      this.formAction = this.onPlaceAsk;
    } else {
      this.label = 'Sell Now';
      this.formAction = this.onSellNow;
      this.form.controls['price'].setValue(this.highestBid);
      this.toastrService.info('Your price matched the highest bid!');
    }

    this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.controls['price'].value);
    this.totalPrice = this.fee + this.form.controls['price'].value;
  }
}
