import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { ApiPaths } from "../api-paths";

@Injectable({
  providedIn: 'root'
})
export class BidsService {

  constructor(private http: HttpClient) { }

  placeBid(itemId: string, size: string, price: string): Observable<any> {
    return this.http.post(
      environment.apiUrl + ApiPaths.Bids,
      { itemId, size, price });
  }
}
