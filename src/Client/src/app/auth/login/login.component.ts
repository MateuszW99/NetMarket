import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

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
      rememberMe: new FormControl(true)
    });
  }

  onLogin(): void {
    this.isLoading = true;

    this.authService
      .login(
        this.form.value.email,
        this.form.value.password,
        this.form.value.rememberMe
      )
      .subscribe(
        () => {
          this.errorMessage = '';
          this.form.reset();
          this.isLoading = false;
          this.router.navigate(['/']);
        },
        (error) => {
          this.isLoading = false;
          console.log(error);
          if (error.error.errorMessages.length > 0) {
            this.errorMessage = error.error.errorMessages[0];
          } else if (typeof error.error === 'object') {
            this.errorMessage = 'Something went wrong';
          } else {
            this.errorMessage = error.error;
          }
        }
      );
  }
}
