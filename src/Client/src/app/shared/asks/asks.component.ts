import { Component, DoCheck, OnDestroy, OnInit } from '@angular/core';
import { AsksService } from "./asks.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ItemDetails } from "../items/item-details/item-details.model";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { SettingsService } from "../../account/settings.service";
import { FeesService } from "../services/fees/fees.service";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";
import { AuthService } from "../../auth/auth.service";
import { User } from "../../auth/user.model";

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

  constructor(
    private askService: AsksService,
    private settingsService: SettingsService,
    private authService: AuthService,
    private toastrService: ToastrService,
    private feesService: FeesService,
    private router: Router) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.userWantsToPlaceAsk = true;
    this.label = 'Place Ask';
    this.formAction = this.onPlaceAsk;

    this.userSubscription = this.authService.user
      .subscribe((user: User) => {
        this.user = user;
      }
    );

    if (this.user === null) {
      this.router.navigate(['/error']);
    }

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item.id, Validators.nullValidator),
      size: new FormControl(this.size, Validators.nullValidator),
      price: new FormControl('', [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$') ]),
    });

    this.loadingSubscription = this.settingsService.loading
      .subscribe((isLoading) => {
        this.loading = isLoading;
      }
    );

    this.sellerLevelSubscription = this.settingsService.userSellerLevelChanged
      .subscribe((sellerLevel) => {
        this.sellerLevel = sellerLevel;
        this.error = '';
      }
    );

    this.errorSubscription = this.settingsService.errorCatched
      .subscribe((errorMessage) => {
        this.error = errorMessage;
      }
    );

    this.settingsService.getUserSellerLevel();

    this.feeSubscription = this.form.controls['price'].valueChanges.pipe(
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
    this.askService.placeAsk(this.form.controls['item'].value, this.form.controls['size'].value, this.form.controls['price'].value.toString())
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrService.success('Ask placed!'));
      });
  }

  onSellNow(): void {
    // TODO: implement
  }

  ngOnDestroy(): void {
    this.sellerLevelSubscription.unsubscribe();
    this.userSubscription.unsubscribe();
    this.feeSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }

  private onPriceChange(): void {
    const userPrice = this.form.controls['price'].value;
    const highestBidPrice = +this.itemDetails.highestBid.price;
    this.userWantsToPlaceAsk = userPrice > highestBidPrice;

    if (this.userWantsToPlaceAsk) {
      this.label = 'Place Ask';
      this.formAction = this.onPlaceAsk;
    } else {
      if (userPrice === highestBidPrice) {
        return;
      }
      this.label = 'Sell Now';
      this.formAction = this.onSellNow;
      this.form.controls['price'].setValue(highestBidPrice);
      this.toastrService.info('Your price matched the highest bid!');
    }

    this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.controls['price'].value);
    this.totalPrice = this.fee + this.form.controls['price'].value;
  }
}
