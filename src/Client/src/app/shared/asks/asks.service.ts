import { Injectable } from '@angular/core';
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { ApiPaths } from "../api-paths";
import { environment } from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AsksService {

  constructor(private http: HttpClient) { }

  placeAsk(itemId: string, size: string, price: string): Observable<any> {
    return this.http.post(
      environment.apiUrl + ApiPaths.Asks,
      { itemId, size, price });
  }

  sellNow() {

  }

}
