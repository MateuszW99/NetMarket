import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiPaths } from '../api-paths';
import { PagedList } from '../paged-list';
import { Bid } from '../bid.model';
import { UpdateOrder } from 'src/app/account/user-orders-table/order-edit/update-order';

@Injectable({
  providedIn: 'root'
})
export class BidsService {
  userBidsChanged = new Subject<PagedList<Bid>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getUserBids(pageIndex: number): void {
    this.loading.next(true);

    let params = new HttpParams();
    params = params.append('pageIndex', pageIndex);
    params = params.append('pageSize', 10);

    this.fetchUserBids(params).subscribe(
      (response: HttpResponse<PagedList<Bid>>) => {
        const bids = new PagedList<Bid>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );
        this.userBidsChanged.next(bids);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading the bids');
        this.loading.next(false);
      }
    );
  }

  private fetchUserBids(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Bid>>> {
    return this.http.get<PagedList<Bid>>(environment.apiUrl + ApiPaths.Bids, {
      observe: 'response',
      params: params
    });
  }

  placeBid(itemId: string, size: string, price: string): Observable<any> {
    return this.http.post(environment.apiUrl + ApiPaths.Bids, {
      itemId,
      size,
      price
    });
  }

  updateBid(bid: UpdateOrder): Observable<unknown> {
    return this.http.put<unknown>(
      environment.apiUrl + ApiPaths.Bids + `/${bid.id}`,
      bid
    );
  }

  deleteBid(bidId: string): Observable<unknown> {
    return this.http.delete<unknown>(
      environment.apiUrl + ApiPaths.Bids + `/${bidId}`
    );
  }
}
