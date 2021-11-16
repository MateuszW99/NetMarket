import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiPaths } from '../../api-paths';
import { Transaction } from 'src/app/supervisor-panel/transaction';
import { PagedList } from '../../paged-list';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  userTransactionsChanged = new Subject<PagedList<Transaction>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getUserTransactions(pageIndex: number, searchText?: string): void {
    this.loading.next(true);

    let params = new HttpParams();
    params = params.append('pageIndex', pageIndex);
    params = params.append('pageSize', 10);
    if (searchText && searchText.trim() !== '') {
      params = params.append('searchQuery', searchText);
    }

    this.fetchUserTransactions(params).subscribe(
      (response: HttpResponse<PagedList<Transaction>>) => {
        const transactions = new PagedList<Transaction>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );
        this.userTransactionsChanged.next(transactions);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading transactions');
        this.loading.next(false);
      }
    );
  }

  sellNow(bidId: string): Observable<unknown> {
    const askId = '';
    return this.http.post(environment.apiUrl + ApiPaths.Transactions, {
      askId,
      bidId
    });
  }

  buyNow(askId: string): Observable<unknown> {
    const bidId = '';
    return this.http.post(environment.apiUrl + ApiPaths.Transactions, {
      askId,
      bidId
    });
  }

  private fetchUserTransactions(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Transaction>>> {
    return this.http.get<PagedList<Transaction>>(
      environment.apiUrl + ApiPaths.Transactions,
      {
        observe: 'response',
        params: params
      }
    );
  }
}
