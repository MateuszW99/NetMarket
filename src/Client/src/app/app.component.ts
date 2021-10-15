import { Component, DoCheck, OnInit } from '@angular/core';
import { AuthService } from './auth/auth.service';
import { RoutingService } from './shared/services/routing/routing.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, DoCheck {
  title = 'Client';
  public isAuthPage: boolean;
  constructor(
    private authService: AuthService,
    private routingService: RoutingService
  ) {}

  ngOnInit(): void {
    this.authService.autoLogin();
  }

  ngDoCheck(): void {
    this.isAuthPage = this.routingService.getCurrentRoute().includes('auth');
  }
}
