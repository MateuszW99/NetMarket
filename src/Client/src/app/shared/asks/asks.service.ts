import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { ApiPaths } from '../api-paths';
import { environment } from '../../../environments/environment';
import { Ask } from '../ask.model';
import { PagedList } from '../paged-list';
import { UpdateOrder } from 'src/app/account/user-orders-table/order-edit/update-order';

@Injectable({
  providedIn: 'root'
})
export class AsksService {
  userAsksChanged = new Subject<PagedList<Ask>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();
  constructor(private http: HttpClient) {}

  placeAsk(itemId: string, size: string, price: string): Observable<any> {
    return this.http.post(environment.apiUrl + ApiPaths.Asks, {
      itemId,
      size,
      price
    });
  }

  sellNow() {

  }

  getUserAsks(pageIndex: number): void {
    this.loading.next(true);

    let params = new HttpParams();
    params = params.append('pageIndex', pageIndex);
    params = params.append('pageSize', 10);

    this.fetchUserAsks(params).subscribe(
      (response: HttpResponse<PagedList<Ask>>) => {
        const asks = new PagedList<Ask>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );
        this.userAsksChanged.next(asks);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading the asks');
        this.loading.next(false);
      }
    );
  }

  updateAsk(ask: UpdateOrder): Observable<unknown> {
    return this.http.put<unknown>(
      environment.apiUrl + ApiPaths.Asks + `/${ask.id}`,
      ask
    );
  }

  deleteAsk(askId: string): Observable<unknown> {
    return this.http.delete<unknown>(
      environment.apiUrl + ApiPaths.Asks + `/${askId}`
    );
  }

  private fetchUserAsks(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Ask>>> {
    return this.http.get<PagedList<Ask>>(environment.apiUrl + ApiPaths.Asks, {
      observe: 'response',
      params: params
    });
  }
}
