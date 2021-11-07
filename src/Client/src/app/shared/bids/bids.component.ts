import { Component, OnDestroy, OnInit } from '@angular/core';
import { ItemDetails } from "../items/item-details/item-details.model";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BidsService } from "./bids.service";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import { SettingsService } from "../../account/settings.service";
import { AuthService } from "../../auth/auth.service";
import { FeesService } from "../services/fees/fees.service";
import { Subject, Subscription } from "rxjs";
import { User } from "../../auth/user.model";
import { catchError, debounceTime, distinctUntilChanged, takeUntil } from "rxjs/operators";
import { TransactionService } from "../services/orders/transaction.service";
import {UpdateOrder} from "../../account/user-orders-table/order-edit/update-order";

@Component({
  selector: 'app-bids',
  templateUrl: './bids.component.html',
  styleUrls: ['./bids.component.css']
})
export class BidsComponent implements OnInit, OnDestroy {

  loading = true;
  loadingSubscription: Subscription;
  error = '';
  errorSubscription: Subscription;

  user: User;
  userSubscription: Subscription;
  sellerLevel: string = '';
  sellerLevelSubscription: Subscription;
  userWantsToPlaceBid: boolean;
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
  private defaultLabel: string = 'Place Bid';
  private transactionLabel = 'Buy Now';
  private lowestAsk: number;

  constructor(
    private bidsService: BidsService,
    private settingsService: SettingsService,
    private authService: AuthService,
    private toastrService: ToastrService,
    private feesService: FeesService,
    private transactionService: TransactionService,
    private router: Router) { }

  ngOnInit(): void {
    if (history.state.data === undefined) {
      this.router.navigate(['..']);
      return;
    }

    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;
    let price = undefined;
    if (this.itemDetails.lowestAsk){
      this.lowestAsk = +this.itemDetails.lowestAsk.price;
    }
    else if (history.state.data.id) {
      this.id = history.state.data.id;
      this.canUpdate = true;
      this.defaultLabel = 'Update Bid';
      price = history.state.data.price;
      this.lowestAsk = +history.state.data.item.item.lowestAsk;
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

    this.userWantsToPlaceBid = true;
    this.label = this.defaultLabel;
    this.formAction = this.onPlaceBid;

    this.userSubscription = this.authService.user
      .subscribe((user: User) => {
          this.user = user;
        }
      );

    if (this.user === null) {
      this.router.navigate(['/auth']);
    }

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

  onPlaceBid(): void {
    if (this.canUpdate) {
      this.bidsService.updateBid(new UpdateOrder(this.id, this.form.controls['size'].value, this.form.controls['price'].value.toString()))        .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrService.success('Bid updated!'));
      })
    } else {
      this.bidsService.placeBid(this.form.controls['item'].value, this.form.controls['size'].value, this.form.controls['price'].value.toString())
        .subscribe(() => {
          this.router.navigate([`/items/${this.itemDetails.item.id}`])
            .then(() => this.toastrService.success('Bid placed!'));
        });
    }
  }

  onBuyNow(): void {
    this.transactionService.buyNow(this.itemDetails.lowestAsk.id)
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
        this.toastrService.success('Congratulations! You just bought an item!');
      });
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  private onPriceChange(): void {
    const userPrice = this.form.controls['price'].value;
    if (!this.lowestAsk) {
      this.userWantsToPlaceBid = true;
    } else {
      this.userWantsToPlaceBid = userPrice < this.lowestAsk;
    }

    if (this.userWantsToPlaceBid && userPrice !== this.lowestAsk) {
      this.toastrService.clear();
      this.label = this.defaultLabel
      this.formAction = this.onPlaceBid;
    } else {
      this.label = this.transactionLabel;
      this.formAction = this.onBuyNow;
      this.form.controls['price'].setValue(this.lowestAsk);
      this.toastrService.info('Your price matched the lowest ask!');
    }

    this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.controls['price'].value);
    this.totalPrice = this.fee + this.form.controls['price'].value;
  }
}
