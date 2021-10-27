import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { User } from 'src/app/auth/user.model';
import { Bid } from 'src/app/shared/bid.model';
import { PagedList } from 'src/app/shared/paged-list';
import { BidsService } from './bids.service';

@Component({
  selector: 'app-bids',
  templateUrl: './bids.component.html',
  styleUrls: ['./bids.component.css']
})
export class BidsComponent implements OnInit, OnDestroy {
  userSubscription: Subscription;
  bidsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  user: User;
  bids: Bid[];

  constructor(
    private authService: AuthService,
    private bidsService: BidsService
  ) {}

  ngOnInit(): void {
    this.userSubscription = this.authService.user.subscribe((user: User) => {
      this.user = user;
    });

    this.bidsService.getUserBids();

    this.loadingSubscription = this.bidsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.bidsService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );

    this.bidsSubscription = this.bidsService.userBidsChanged.subscribe(
      (bids: PagedList<Bid>) => {
        this.bids = bids.items;
        console.log(this.bids);
      }
    );
  }

  ngOnDestroy(): void {
    this.userSubscription.unsubscribe();
    this.bidsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
