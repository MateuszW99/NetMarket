import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ItemDetails } from '../shared/items/item-details/item-details.model';
import { ItemsService } from '../shared/items/items.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css']
})
export class LandingPageComponent implements OnInit, OnDestroy {
  trendingItems: Map<string, ItemDetails[]> = new Map([
    ['sneakers', []],
    ['streetwear', []],
    ['electronics', []],
    ['collectibles', []]
  ]);
  categories = [...this.trendingItems.keys()];
  trendingItemsSubscription: Subscription;
  error = false;
  loading = true;

  constructor(private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.categories.forEach((category: string) => {
      this.loading = true;
      this.trendingItemsSubscription = this.itemsService
        .getTrendingItems(category, 12)
        .subscribe(
          (items: ItemDetails[]) => {
            this.trendingItems.set(category, items);
            this.error = false;
            this.loading = false;
          },
          () => {
            this.error = true;
            this.loading = false;
          }
        );
    });
  }

  ngOnDestroy(): void {
    this.trendingItemsSubscription.unsubscribe();
  }
}
