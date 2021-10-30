import {Component, OnDestroy, OnInit} from '@angular/core';
import { ItemDetails } from "../items/item-details/item-details.model";
import { Size } from "../size.model";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { BidsService } from "./bids.service";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import {SettingsService} from "../../account/settings.service";
import {AuthService} from "../../auth/auth.service";
import {FeesService} from "../services/fees/fees.service";
import {Subscription} from "rxjs";
import {User} from "../../auth/user.model";
import {debounceTime, distinctUntilChanged} from "rxjs/operators";

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

  itemDetails: ItemDetails;
  size: Size;
  form: FormGroup;
  userWantsToPlaceBid: boolean;
  fee: number = 0;
  feeSubscription: Subscription;

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
      price: new FormControl('', [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]), // TODO: offer new highestBid
    });

    this.userWantsToPlaceBid = this.form.get('price').value >= this.itemDetails.lowestAsk.price;

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

    this.feeSubscription = this.form.valueChanges.pipe(
      debounceTime(1000),
      distinctUntilChanged()
    ).subscribe(() => {
      //this.settingsService.getUserSellerLevel();
      this.fee = this.feesService.calculateFees(this.sellerLevel, this.form.value.price);
    });
  }

  getLabel() {
    return this.userWantsToPlaceBid ? 'Place Bid' : 'Buy';
  }

  onSubmitForm(): void {
    if (this.userWantsToPlaceBid) {
      this.onPlaceBid();
    }
    else {
      this.onBuyNow();
    }
  }

  onPlaceBid(): void {
    this.bidsService.placeBid(this.form.value.item, this.form.value.size, this.form.value.price.toString())
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

}
