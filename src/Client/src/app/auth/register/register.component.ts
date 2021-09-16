import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomValidators } from 'src/app/shared/custom-validators';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
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
      passwords: new FormGroup(
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
      ),
      username: new FormControl('', [
        Validators.minLength(1),
        Validators.maxLength(30),
        Validators.required
      ])
    });
  }

  onRegister(): void {
    this.authService
      .register(
        this.form.value.username,
        this.form.value.email,
        this.form.value.passwords.password
      )
      .subscribe(
        () => {
          console.log('registered');

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
