import { Component, Input } from '@angular/core';
import { SellerLevels } from '../seller-levels';

@Component({
  selector: 'app-seller-level-progress',
  templateUrl: './seller-level-progress.component.html',
  styleUrls: ['./seller-level-progress.component.css']
})
export class SellerLevelProgressComponent {
  @Input() salesCompleted: number;
  @Input() sellerLevel: string;
  sellerLevels = SellerLevels.sellerLevels;
  salesNeeded: Map<string, number> = new Map([
    ['Beginner', 3],
    ['Intermediate', 10],
    ['Advanced', 25],
    ['Business', 100]
  ]);

  getNeededSalesNumber(): number {

    if (this.salesCompleted < 100) {
      let salesNeeded = this.salesNeeded.get(this.sellerLevel) - this.salesCompleted;
      
      if(salesNeeded < 0)
        return 0;
      
      return salesNeeded;
    }

    return 0;
  }

  getProgressBarWidth(): string {
    const width =
      (this.salesCompleted / this.salesNeeded.get(this.sellerLevel)) * 100;
    return width.toString() + '%';
  }
}
