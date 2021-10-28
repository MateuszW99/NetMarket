import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { SettingsService } from 'src/app/account/settings.service';
import { UserSettings } from 'src/app/account/user-settings.model';

@Component({
  selector: 'app-shipping-address-edit',
  templateUrl: './shipping-address-edit.component.html',
  styleUrls: ['./shipping-address-edit.component.css']
})
export class ShippingAddressEditComponent implements OnInit {
  userSettings: UserSettings;
  form: FormGroup;
  updateUserSettingsSubscription: Subscription;
  error = false;

  constructor(
    private settingsService: SettingsService,
    @Inject(MAT_DIALOG_DATA) public data: { userSettings: UserSettings },
    private dialogRef: MatDialogRef<ShippingAddressEditComponent>
  ) {}

  ngOnInit(): void {
    this.userSettings = this.data.userSettings;
    this.form = new FormGroup({
      shippingStreet: new FormControl(this.userSettings.shippingStreet, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      shippingAddressLine1: new FormControl(
        this.userSettings.shippingAddressLine1,
        [Validators.required, Validators.minLength(2), Validators.maxLength(50)]
      ),
      shippingAddressLine2: new FormControl(
        this.userSettings.shippingAddressLine2,
        Validators.maxLength(50)
      ),
      shippingZipCode: new FormControl(this.userSettings.shippingZipCode, [
        Validators.pattern('^[0-9]{2}-?[0-9]{3}$'),
        Validators.required,
        Validators.maxLength(6)
      ]),
      shippingCountry: new FormControl(this.userSettings.shippingCountry, [
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
