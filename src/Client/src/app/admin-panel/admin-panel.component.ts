import { BreakpointObserver } from '@angular/cdk/layout';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Subscription } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { User } from '../auth/user.model';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css']
})
export class AdminPanelComponent implements OnInit {
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
