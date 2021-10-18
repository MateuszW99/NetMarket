import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';
import { SettingsService } from '../settings.service';
import { UserSettings } from '../user-settings.model';

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
  shippingAddressProvided = false;
  billingAddressProvided = false;

  constructor(
    private settingsService: SettingsService,
    private authService: AuthService
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
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.userSettingsSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }
}
