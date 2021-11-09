import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent implements OnInit, OnDestroy {
  role: string;
  userSubscription: Subscription
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.userSubscription = this.authService.user.subscribe((user: User) => {
      if(user != null){
        this.role = user.role;
      }
      else{
        this.role === 'notLoggedIn';
      }
    }) 
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
  }

}
