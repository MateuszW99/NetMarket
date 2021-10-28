import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {Observable, of} from "rxjs";
import { environment } from "../../../../environments/environment";
import { ApiPaths } from "../../api-paths";

@Injectable({
  providedIn: 'root'
})
export class FeesService {

  constructor(private http: HttpClient) { }

  private mapSellerLevelToFeeRate(sellerLevel: string = 'Beginner'): number {
    switch (sellerLevel) {
      case 'Beginner': return 0.1;
      case 'Intermediate': return 0.085;
      case 'Advanced': return 0.06;
      case 'Business': return 0.04;
      default: return 0.1;
    }
  }

  calculateFees(price: number): Observable<number> {
    this.http.get<string>(environment.apiUrl + ApiPaths.UserSettings + '/level')
      .subscribe(x => {
        return of(price * this.mapSellerLevelToFeeRate(x));
      });
    return of(price * this.mapSellerLevelToFeeRate());
  }
}
