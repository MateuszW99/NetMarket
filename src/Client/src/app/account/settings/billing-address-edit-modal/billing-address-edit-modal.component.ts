import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../settings.service';
import { UserSettings } from '../../user-settings.model';

@Component({
  selector: 'app-billing-address-edit-modal',
  templateUrl: './billing-address-edit-modal.component.html',
  styleUrls: ['./billing-address-edit-modal.component.css']
})
export class BillingAddressEditModalComponent implements OnInit {
  userSettings: UserSettings;
  form: FormGroup;
  updateUserSettingsSubscription: Subscription;
  error = false;

  constructor(
    private settingsService: SettingsService,
    @Inject(MAT_DIALOG_DATA) public data: { userSettings: UserSettings },
    private dialogRef: MatDialogRef<BillingAddressEditModalComponent>
  ) {}

  ngOnInit(): void {
    this.userSettings = this.data.userSettings;
    this.form = new FormGroup({
      paypalEmail: new FormControl(this.userSettings.paypalEmail, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(40),
        Validators.email
      ]),
      billingStreet: new FormControl(this.userSettings.billingStreet, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      billingAddressLine1: new FormControl(
        this.userSettings.billingAddressLine1,
        [Validators.required, Validators.minLength(2), Validators.maxLength(50)]
      ),
      billingAddressLine2: new FormControl(
        this.userSettings.billingAddressLine2,
        Validators.maxLength(50)
      ),
      billingZipCode: new FormControl(this.userSettings.billingZipCode, [
        Validators.pattern('^[0-9]{2}-?[0-9]{3}$'),
        Validators.required,
        Validators.maxLength(6)
      ]),
      billingCountry: new FormControl(this.userSettings.billingCountry, [
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
