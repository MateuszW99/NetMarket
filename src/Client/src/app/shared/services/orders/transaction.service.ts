import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../../environments/environment";
import { ApiPaths } from "../../api-paths";

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  constructor(private http: HttpClient) { }

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
}
