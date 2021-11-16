import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../shared/api-paths';
import { PagedList } from '../shared/paged-list';
import { Transaction } from './transaction';
import { UpdateTransactionStatus } from './update-transaction-status';

@Injectable({
  providedIn: 'root'
})
export class SupervisorService {
  transactionsChanged = new Subject<PagedList<Transaction>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getAssignedTransactions(pageIndex: number, status?: string): void {
    this.loading.next(true);

    let params = new HttpParams();
    params = params.append('pageIndex', pageIndex);
    params = params.append('pageSize', 10);
    if (status && status.trim() !== '') {
      params = params.append('status', status);
    }

    this.fetchAssignedTransactions(params).subscribe(
      (response: HttpResponse<PagedList<Transaction>>) => {
        const transactions = new PagedList<Transaction>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );

        this.transactionsChanged.next(transactions);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next(
          'An eror occured while loading assigned transactions'
        );
        this.loading.next(false);
      }
    );
  }

  updateTransactionStatus(
    transaction: UpdateTransactionStatus
  ): Observable<unknown> {
    return this.http.put<unknown>(
      environment.apiUrl +
        ApiPaths.SupervisorPanel +
        ApiPaths.Orders +
        `/${transaction.id}`,
      transaction
    );
  }

  getTransaction(transactionId: string): Observable<Transaction> {
    return this.http.get<Transaction>(
      environment.apiUrl +
        ApiPaths.SupervisorPanel +
        ApiPaths.Orders +
        `/${transactionId}`
    );
  }

  private fetchAssignedTransactions(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Transaction>>> {
    return this.http.get<PagedList<Transaction>>(
      environment.apiUrl + ApiPaths.SupervisorPanel + ApiPaths.Orders,
      {
        observe: 'response',
        params: params
      }
    );
  }
}
