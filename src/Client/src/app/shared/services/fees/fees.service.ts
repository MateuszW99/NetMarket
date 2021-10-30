import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FeesService {

  constructor() { }

  private mapSellerLevelToFeeRate(sellerLevel: string = 'Beginner'): number {
    switch (sellerLevel) {
      case 'Beginner': return 0.1;
      case 'Intermediate': return 0.085;
      case 'Advanced': return 0.06;
      case 'Business': return 0.04;
      default: return 0.1;
    }
  }

  calculateFees(sellerLevel: string, price: number): number {
    return this.mapSellerLevelToFeeRate(sellerLevel) * price;
  }
}
