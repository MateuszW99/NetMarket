import { Component, Input } from '@angular/core';
import { ItemDetails } from 'src/app/shared/items/item-details/item-details.model';

@Component({
  selector: 'app-trending-items',
  templateUrl: './trending-items.component.html',
  styleUrls: ['./trending-items.component.css']
})
export class TrendingItemsComponent {
  @Input() category = '';
  @Input() items: ItemDetails[];
}
