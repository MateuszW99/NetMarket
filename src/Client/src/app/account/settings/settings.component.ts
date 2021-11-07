import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import Modal from 'bootstrap/js/dist/modal';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';
import { SettingsService } from '../settings.service';
import { UserSettings } from '../user-settings.model';
import { BillingAddressEditComponent } from './edit-modals/billing-address-edit/billing-address-edit.component';
import { ProfileEditComponent } from './edit-modals/profile-edit/profile-edit.component';
import { ShippingAddressEditComponent } from './edit-modals/shipping-address-edit/shipping-address-edit.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit, OnDestroy {
  user: User;
  userSettings: UserSettings;
  error = '';
  loading = true;
  userSubscription: Subscription;
  userSettingsSubscription: Subscription;
  errorSubscription: Subscription;
  loadingSubscription: Subscription;
  profileInfoProvided = false;
  shippingAddressProvided = false;
  billingAddressProvided = false;

  constructor(
    private settingsService: SettingsService,
    private authService: AuthService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.userSubscription = this.authService.user.subscribe((user: User) => {
      this.user = user;
    });

    this.settingsService.getUserSettings();

    this.loadingSubscription = this.settingsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.userSettingsSubscription =
      this.settingsService.userSettingsChanged.subscribe(
        (settings: UserSettings) => {
          this.userSettings = settings;
          this.checkSettings(this.userSettings);
          this.error = '';
        }
      );

    this.errorSubscription = this.settingsService.errorCatched.subscribe(
      (errorMessage) => {
        this.error = errorMessage;
      }
    );
  }

  checkSettings(userSettings: UserSettings): void {
    if (
      !userSettings.billingStreet ||
      !userSettings.billingAddressLine1 ||
      !userSettings.billingZipCode ||
      !userSettings.billingCountry
    ) {
      this.billingAddressProvided = false;
    } else {
      this.billingAddressProvided = true;
    }

    if (
      !userSettings.shippingStreet ||
      !userSettings.shippingAddressLine1 ||
      !userSettings.shippingZipCode ||
      !userSettings.shippingCountry
    ) {
      this.shippingAddressProvided = false;
    } else {
      this.shippingAddressProvided = true;
    }

    if (!userSettings.firstName || !userSettings.lastName) {
      this.profileInfoProvided = false;
    } else {
      this.profileInfoProvided = true;
    }
  }

  openResetPassword(): void {
    this.dialog.open(ResetPasswordComponent, {
      width: '600px'
    });
  }

  openBillingEdit(): void {
    this.dialog.open(BillingAddressEditComponent, {
      width: '600px',
      data: { userSettings: this.userSettings }
    });
  }

  openShippingEdit(): void {
    this.dialog.open(ShippingAddressEditComponent, {
      width: '600px',
      data: { userSettings: this.userSettings }
    });
  }

  openProfileEdit(): void {
    this.dialog.open(ProfileEditComponent, {
      width: '600px',
      data: { userSettings: this.userSettings }
    });
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.userSettingsSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }
}
