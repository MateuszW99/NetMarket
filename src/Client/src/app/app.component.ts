import {
  Component,
  DoCheck,
  OnChanges,
  OnInit,
  SimpleChanges
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { AuthService } from './auth/auth.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, DoCheck {
  title = 'Client';
  public isAuthPage: boolean;
  constructor(private authService: AuthService, private location: Location) {}

  ngOnInit(): void {
    this.authService.autoLogin();
    console.log(this.location.path());
    this.isAuthPage = this.location.path().includes('auth');
  }

  ngDoCheck(): void {
    this.isAuthPage = this.location.path().includes('auth');
  }
}
