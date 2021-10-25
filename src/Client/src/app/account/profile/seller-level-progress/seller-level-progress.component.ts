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
  sellerLevelKeys = SellerLevels.sellerLevelKeys;
}
