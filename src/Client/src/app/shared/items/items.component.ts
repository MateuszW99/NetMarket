import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { PagedList } from '../paged-list';
import { Item } from './item.model';
import { ItemsParams } from './items-params';
import { ItemsService } from './items.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit, OnDestroy {
  items: PagedList<Item>;
  error = '';
  loading = false;
  empty = true;
  itemsSubscription: Subscription;
  errorSubscription: Subscription;
  loadingSubscription: Subscription;

  constructor(private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.loadingSubscription = this.itemsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.itemsService.getItems(new ItemsParams(15, 1, 'sneakers'));

    this.itemsSubscription = this.itemsService.itemsChanged.subscribe(
      (items) => {
        this.empty = items.totalCount === 0;
        this.items = items;
      }
    );

    this.errorSubscription = this.itemsService.errorCatched.subscribe(
      (errorMessage) => {
        this.error = errorMessage;
      }
    );
  }

  ngOnDestroy(): void {
    this.itemsSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
  }
}
