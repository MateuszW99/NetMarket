import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ApiPaths } from 'src/app/shared/api-paths';
import { Bid } from 'src/app/shared/bid.model';
import { PagedList } from 'src/app/shared/paged-list';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BidsService {
  userBidsChanged = new Subject<PagedList<Bid>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getUserBids(): void {
    this.loading.next(true);
    this.fetchUserBids().subscribe(
      (bids: PagedList<Bid>) => {
        this.userBidsChanged.next(bids);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading the bids');
        this.loading.next(false);
      }
    );
  }

  private fetchUserBids(): Observable<PagedList<Bid>> {
    return this.http.get<PagedList<Bid>>(environment.apiUrl + ApiPaths.Bids);
  }
}
