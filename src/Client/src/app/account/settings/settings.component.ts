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
  loading = false;
  userSubscription: Subscription;
  userSettingsSubscription: Subscription;
  errorSubscription: Subscription;
  loadingSubscription: Subscription;

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
          this.error = '';
        }
      );

    this.errorSubscription = this.settingsService.errorCatched.subscribe(
      (errorMessage) => {
        this.error = errorMessage;
      }
    );
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.userSettingsSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }
}
