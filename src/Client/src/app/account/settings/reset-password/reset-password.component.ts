import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { CustomValidators } from 'src/app/shared/custom-validators';
import { ResetPasswordRequest } from './reset-password-request';
import { ResetPasswordResponse } from './resset-password-result';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit, OnDestroy {
  form: FormGroup;
  error = '';
  reset = false;
  resetPasswordSubscription: Subscription;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl('', [
        Validators.email,
        Validators.required,
        Validators.maxLength(256)
      ]),
      password: new FormControl('', [
        Validators.minLength(8),
        Validators.maxLength(64),
        Validators.required
      ]),
      newPasswords: new FormGroup(
        {
          password: new FormControl('', [
            Validators.minLength(8),
            Validators.maxLength(64),
            Validators.required
          ]),
          confirmPassword: new FormControl('', [
            Validators.minLength(8),
            Validators.maxLength(64),
            Validators.required
          ])
        },
        CustomValidators.samePasswords
      )
    });
  }

  onSubmit(): void {
    const request = new ResetPasswordRequest(
      this.form.value.email,
      this.form.value.password,
      this.form.value.newPasswords.password
    );

    this.resetPasswordSubscription = this.authService
      .resetPassword(request)
      .subscribe(
        (response: ResetPasswordResponse) => {
          if (response.success) {
            this.reset = true;
          } else {
            this.error = response.errorMessages[0];
          }
        },
        (error: HttpErrorResponse) => {
          this.error = error.error.errorMessages[0];
        }
      );
  }

  ngOnDestroy(): void {
    if (this.resetPasswordSubscription) {
      this.resetPasswordSubscription.unsubscribe();
    }
  }
}
