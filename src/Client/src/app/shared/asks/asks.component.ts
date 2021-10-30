import {Component, OnDestroy, OnInit} from '@angular/core';
import { AsksService } from "./asks.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ItemDetails } from "../items/item-details/item-details.model";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";
import {Subscription} from "rxjs";
import {SettingsService} from "../../account/settings.service";
import {FeesService} from "../services/fees/fees.service";
import {debounceTime, distinctUntilChanged} from "rxjs/operators";
import {AuthService} from "../../auth/auth.service";
import {User} from "../../auth/user.model";

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

  form: FormGroup;
  itemDetails: ItemDetails;
  size: string;
  userWantsToPlaceAsk: boolean;
  fee: number = 0;
  feeSubscription: Subscription;

  constructor(
    private askService: AsksService,
    private settingsService: SettingsService,
    private authService: AuthService,
    private toastrSerivce: ToastrService,
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
      price: new FormControl('', [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]), // TODO: offer newLowestAsk
    });

    //this.userWantsToPlaceAsk = this.form.get('price').value >= this.itemDetails.lowestAsk.price;



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

  ngOnDestroy(): void {
    this.sellerLevelSubscription.unsubscribe();
    this.userSubscription.unsubscribe();
    this.feeSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }

  isNewLowestAsk(): boolean {
    return this.form.get('price').value <= this.itemDetails.lowestAsk.price;
  }

  getLabel() {
    return this.userWantsToPlaceAsk ? 'Place Ask' : 'Sell';
  }

  onSubmitForm(): void {
    if (this.userWantsToPlaceAsk) {
      this.onPlaceAsk();
    }
    else {
      this.onSellNow();
    }
  }

  onPlaceAsk(): void {
    this.askService.placeAsk(this.form.value.item, this.form.value.size, this.form.value.price.toString())
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrSerivce.success('Ask placed!'));
      });
  }

  onSellNow(): void {

  }


}
