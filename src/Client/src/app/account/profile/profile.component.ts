import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';
import { SettingsService } from '../settings.service';
import { UserSettings } from '../user-settings.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {
  user: User;
  userSettings: UserSettings;
  loading = true;
  userSubscription: Subscription;
  settingsSubscription: Subscription;
  loadingSubscription: Subscription;
  categories = ['sneakers', 'streetwear', 'electronics', 'collectibles'];

  constructor(
    private authService: AuthService,
    private settingsService: SettingsService
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

    this.settingsSubscription =
      this.settingsService.userSettingsChanged.subscribe(
        (settings: UserSettings) => {
          this.userSettings = settings;
        }
      );
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.settingsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }
}
