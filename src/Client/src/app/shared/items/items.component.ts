import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { PagedList } from '../paged-list';
import { Pagination } from '../pagination';
import { Item } from './item.model';
import { ItemsParams } from './items-params';
import { ItemsService } from './items.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit, OnDestroy {
  @Input() category = '';
  items: PagedList<Item>;
  error = '';
  loading = false;
  empty = true;
  itemsParams: ItemsParams;
  itemsSubscription: Subscription;
  errorSubscription: Subscription;
  loadingSubscription: Subscription;
  itemsParamsSubscription: Subscription;

  constructor(private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.loadingSubscription = this.itemsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.itemsParamsSubscription = this.itemsService.paramsChanged.subscribe(
      (params: ItemsParams) => {
        this.itemsParams = params;
      }
    );

    this.itemsService.getItems(new ItemsParams(15, 1, this.category));

    this.itemsSubscription = this.itemsService.itemsChanged.subscribe(
      (items) => {
        this.empty = items.totalCount === 0;
        this.error = '';
        this.items = items;
      }
    );

    this.errorSubscription = this.itemsService.errorCatched.subscribe(
      (errorMessage) => {
        this.error = errorMessage;
      }
    );
  }

  getPaginationData(): Pagination {
    return {
      pageIndex: this.items.pageIndex,
      hasNextPage: this.items.hasNextPage,
      hasPreviousPage: this.items.hasPreviousPage,
      totalPages: this.items.totalPages,
      totalCount: this.items.totalCount
    };
  }

  ngOnDestroy(): void {
    this.itemsSubscription.unsubscribe();
    this.errorSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.itemsParamsSubscription.unsubscribe();
  }
}
