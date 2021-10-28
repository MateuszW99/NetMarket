import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
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
}
