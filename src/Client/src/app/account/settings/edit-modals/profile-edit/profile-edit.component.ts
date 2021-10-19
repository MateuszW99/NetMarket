import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { SettingsService } from 'src/app/account/settings.service';
import { UserSettings } from 'src/app/account/user-settings.model';

@Component({
  selector: 'app-profile-edit',
  templateUrl: './profile-edit.component.html',
  styleUrls: ['./profile-edit.component.css']
})
export class ProfileEditComponent implements OnInit {
  userSettings: UserSettings;
  form: FormGroup;
  updateUserSettingsSubscription: Subscription;
  error = false;

  constructor(
    private settingsService: SettingsService,
    @Inject(MAT_DIALOG_DATA) public data: { userSettings: UserSettings },
    private dialogRef: MatDialogRef<ProfileEditComponent>
  ) {}

  ngOnInit(): void {
    this.userSettings = this.data.userSettings;
    this.form = new FormGroup({
      firstName: new FormControl(this.userSettings.firstName, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      lastName: new FormControl(this.userSettings.lastName, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ])
    });
  }

  onSubmit(): void {
    this.userSettings = this.form.value as UserSettings;

    this.updateUserSettingsSubscription = this.settingsService
      .updateUserSettings(this.userSettings)
      .subscribe(
        () => {
          this.settingsService.getUserSettings();
          this.error = false;
          this.dialogRef.close();
        },
        () => {
          this.error = true;
        }
      );
  }
}
