import {
  HttpEvent,
  HttpHandler,
  HttpHeaders,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { take, exhaustMap, catchError } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.authService.user.pipe(
      take(1),
      exhaustMap((user) => {
        if (!user) {
          return next.handle(req);
        }

        if (
          user.tokenExpirationDate &&
          new Date() > new Date(user.tokenExpirationDate)
        ) {
          this.authService.logout();
          return null;
        }

        const modifiedReq = req.clone({
          headers: new HttpHeaders().set(
            'Authorization',
            `Bearer ${user.token}`
          )
        });
        return next.handle(modifiedReq);
      })
    );
  }
}
