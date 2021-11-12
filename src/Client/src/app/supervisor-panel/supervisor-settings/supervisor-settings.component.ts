import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { SettingsService } from 'src/app/account/settings.service';
import { ProfileEditComponent } from 'src/app/account/settings/edit-modals/profile-edit/profile-edit.component';
import { ResetPasswordComponent } from 'src/app/account/settings/reset-password/reset-password.component';
import { UserSettings } from 'src/app/account/user-settings.model';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';

@Component({
  selector: 'app-supervisor-settings',
  templateUrl: './supervisor-settings.component.html',
  styleUrls: ['./supervisor-settings.component.css']
})
export class SupervisorSettingsComponent implements OnInit {
  user: User;
  userSettings: UserSettings;
  error = '';
  loading = true;
  userSubscription: Subscription;
  userSettingsSubscription: Subscription;
  errorSubscription: Subscription;
  loadingSubscription: Subscription;
  profileInfoProvided = false;

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
