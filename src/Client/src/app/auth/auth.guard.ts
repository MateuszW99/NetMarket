import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree
} from '@angular/router';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    return this.authService.user.pipe(
      take(1),
      map((user) => {
        console.log(user);
        console.log(route.data.roles);
        
        
        const isAuthenticated = !!user;
        if (isAuthenticated) {
          if (
            (route.data.roles && user.role.includes(route.data.roles[0])) ||
            !route.data.roles
          ) {
            return true;
          } else {
            return this.router.createUrlTree(['/auth'], {
              queryParams: { returnUrl: state.url }
            });
          }
        }
        return this.router.createUrlTree(['/auth'], {
          queryParams: { returnUrl: state.url }
        });
      })
    );
  }
}
