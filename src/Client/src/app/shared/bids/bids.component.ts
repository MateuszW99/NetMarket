import {Component, OnDestroy, OnInit} from '@angular/core';
import { ItemDetails } from "../items/item-details/item-details.model";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BidsService } from "./bids.service";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import { SettingsService } from "../../account/settings.service";
import { AuthService } from "../../auth/auth.service";
import { FeesService } from "../services/fees/fees.service";
import { Subscription } from "rxjs";
import { User } from "../../auth/user.model";
import { debounceTime, distinctUntilChanged } from "rxjs/operators";

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

  constructor(
    private bidsService: BidsService,
    private settingsService: SettingsService,
    private authService: AuthService,
    private toastrService: ToastrService,
    private feesService: FeesService,
    private router: Router) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.userWantsToPlaceBid = true;
    this.label = 'Place Bid';
    this.formAction = this.onPlaceBid;

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

  onPlaceBid(): void {
    this.bidsService.placeBid(this.form.controls['item'].value, this.form.controls['size'].value, this.form.controls['price'].value.toString())
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrService.success('Bid placed!'));
      });
  }

  onBuyNow(): void {

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
    const lowestAskPrice = +this.itemDetails.lowestAsk.price;
    this.userWantsToPlaceBid = userPrice < lowestAskPrice;

    if (this.userWantsToPlaceBid) {
      this.label = 'Place Bid';
      this.formAction = this.onPlaceBid;
    } else {
      if (userPrice === lowestAskPrice) {
        return;
      }
      this.label = 'Buy Now';
      this.formAction = this.onBuyNow;
      this.form.controls['price'].setValue(lowestAskPrice);
      this.toastrService.info('Your price matched the lowest ask!');
    }

    this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.controls['price'].value);
    this.totalPrice = this.fee + this.form.controls['price'].value;
  }
}
