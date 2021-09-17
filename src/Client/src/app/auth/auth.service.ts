import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from './user.model';
import jwt_decode from 'jwt-decode';
import { tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthResponse } from './auth-response';
import { TokenClaims } from './token-claims';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../shared/api-paths';

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
      .post<AuthResponse>(
        environment.apiUrl + ApiPaths.Identity + ApiPaths.Register,
        {
          email,
          username,
          password
        }
      )
      .pipe(
        tap((resData) => {
          console.log(resData);

          if (resData.token) {
            this.handleAuthentication(resData.token, true);
          }
        })
      );
  }

  login(
    email: string,
    password: string,
    rememberMe: boolean
  ): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(
        environment.apiUrl + ApiPaths.Identity + ApiPaths.Login,
        {
          email,
          password
        }
      )
      .pipe(
        tap((resData) => {
          if (resData.token) {
            this.handleAuthentication(resData.token, rememberMe);
          }
        })
      );
  }

  logout(): void {
    this.user.next(null);
    this.router.navigate(['/auth']);
    localStorage.removeItem('userData');
    localStorage.removeItem('rememberMe');
  }

  autoLogin(): void {
    if (JSON.parse(localStorage.getItem('RememberMe')) !== null) {
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
  }

  private handleAuthentication(token: string, rememberMe: boolean): void {
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

    if (rememberMe) {
      localStorage.setItem('rememberMe', JSON.stringify(rememberMe));
    }
  }
}
