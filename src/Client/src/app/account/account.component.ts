import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import { BreakpointObserver } from '@angular/cdk/layout';
import { MatSidenav } from '@angular/material/sidenav';
import { AuthService } from '../auth/auth.service';
import { User } from '../auth/user.model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, OnDestroy {
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  user: User;
  userSubscription: Subscription;

  constructor(
    private observer: BreakpointObserver,
    private cdr: ChangeDetectorRef,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userSubscription = this.authService.user.subscribe((user: User) => {
      if (user) {
        if (new Date() > new Date(user.tokenExpirationDate)) {
          this.authService.logout();
        } else {
          this.user = user;
        }
      }
    });
  }

  ngAfterViewInit(): void {
    if (this.user) {
      this.observer.observe(['(max-width: 768px)']).subscribe((res) => {
        if (res.matches) {
          this.sidenav.mode = 'over';
          this.sidenav.close();
        } else {
          this.sidenav.mode = 'side';
          this.sidenav.open();
        }
      });
      this.cdr.detectChanges();
    }
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }
}
