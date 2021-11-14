import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { Roles } from '../roles';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errorMessage = '';
  isLoading = false;
  returnUrl: string;

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

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

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
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
          const role = this.authService.getUserRole();

          if (role === Roles.User) {
            if (
              this.returnUrl.includes('supervisor') ||
              this.returnUrl.includes('admin')
            ) {
              this.router.navigate(['']);
            } else {
              this.router.navigateByUrl(this.returnUrl);
            }
          } else if (role === Roles.Supervisor) {
            this.router.navigate(['/supervisor-panel']);
          } else if (role === Roles.Admin) {
            this.router.navigate(['/admin-panel']);
          }
        },
        (error) => {
          this.isLoading = false;
          if (
            !error.error.errorMessages ||
            error.error.errorMessages.length === 0
          ) {
            this.errorMessage = 'Something went wrong';
          } else {
            this.errorMessage = error.error.errorMessages[0];
          }
        }
      );
  }
}
