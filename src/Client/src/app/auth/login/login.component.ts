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
          this.router.navigate(['/']);
        },
        (error) => {
          console.log(error);

          this.errorMessage = error.error;
        }
      );
  }
}
