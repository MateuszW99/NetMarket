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

  constructor(private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.categories.forEach((category: string) => {
      this.trendingItemsSubscription = this.itemsService
        .getTrendingItems(category, 9)
        .subscribe((items: ItemDetails[]) => {
          this.trendingItems.set(category, items);
        });
    });
  }

  ngOnDestroy(): void {
    this.trendingItemsSubscription.unsubscribe();
  }
}
