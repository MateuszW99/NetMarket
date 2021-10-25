import { Component, Input } from '@angular/core';
import { SellerLevels } from '../seller-levels';

@Component({
  selector: 'app-seller-level',
  templateUrl: './seller-level.component.html',
  styleUrls: ['./seller-level.component.css']
})
export class SellerLevelComponent {
  @Input() sellerLevel: string;
  sellerLevels = SellerLevels.sellerLevels;
  sellerLevelKeys = SellerLevels.sellerLevelKeys;
}
