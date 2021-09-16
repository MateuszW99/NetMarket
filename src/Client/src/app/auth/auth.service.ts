import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from './user.model';
import jwt_decode from 'jwt-decode';
import { tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthResponse } from './authResponse';
import { TokenClaims } from './tokenClaims';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private router: Router) {}

  user = new BehaviorSubject<User>(null);

  register(
    email: string,
    username: string,
    password: string
  ): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>('https://localhost5001/api' + '/identity/register', {
        username,
        email,
        password
      })
      .pipe(
        tap((resData) => {
          console.log(resData);

          if (resData.token) {
            this.handleAuthentication(resData.token);
          }
        })
      );
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>('https://localhost:5001/api' + '/identity/login', {
        email,
        password
      })
      .pipe(
        tap((resData) => {
          if (resData.token) {
            this.handleAuthentication(resData.token);
          }
        })
      );
  }

  logout(): void {
    this.user.next(null);
    this.router.navigate(['/auth']);
    localStorage.removeItem('userData');
  }

  autoLogin(): void {
    // get user from local storage
    const userData = JSON.parse(localStorage.getItem('userData'));
    if (!userData) {
      return;
    }

    const loadedUser = new User(
      userData.id,
      userData.email,
      userData.role,
      userData._token,
      userData._tokenExpirationDate
    );

    if (loadedUser.token) {
      this.user.next(loadedUser);
    }
  }

  private handleAuthentication(token: string): void {
    const decodedToken: TokenClaims = jwt_decode(token);

    console.log(decodedToken.id);
    console.log(decodedToken.email);
    console.log(decodedToken.role);
    console.log(decodedToken.exp);

    const expirationDate = new Date(+decodedToken.exp * 1000);

    const user = new User(
      decodedToken.id,
      decodedToken.email,
      decodedToken.role,
      token,
      expirationDate
    );
    this.user.next(user);

    localStorage.setItem('userData', JSON.stringify(user));
  }
}
