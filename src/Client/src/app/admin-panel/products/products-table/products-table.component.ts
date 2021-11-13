import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Item } from 'src/app/shared/items/item.model';
import { ItemsParams } from 'src/app/shared/items/items-params';
import { ItemsService } from 'src/app/shared/items/items.service';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { ItemRow } from './item-row';

@Component({
  selector: 'app-products-table',
  templateUrl: './products-table.component.html',
  styleUrls: ['./products-table.component.css']
})
export class ProductsTableComponent implements OnInit, OnDestroy {
  category = 'sneakers';
  searchText = '';
  dataSource = new MatTableDataSource<ItemRow>();
  itemsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  paginationData: Pagination;
  pageEvent: PageEvent;
  displayedColumns = ['id', 'name', 'brand', 'gender', 'retailPrice'];

  constructor(public dialog: MatDialog, private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.itemsService.getItems(
      new ItemsParams(10, 1, this.category, this.searchText)
    );
    this.loadingSubscription = this.itemsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.itemsService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.itemsSubscription = this.itemsService.itemsChanged.subscribe(
      (items: PagedList<Item>) => {
        this.dataSource = new MatTableDataSource<ItemRow>(this.getRows(items));
        this.paginationData = this.getPaginationData(items);
      }
    );
  }

  onManage(row: ItemRow): void {
    console.log('on manage clicked');

    // this.dialog.open(ManageItemComponent, {
    //   width: '600px',
    //   data: { itemId: row.id }
    // });
  }

  getRows(data: PagedList<Item>): ItemRow[] {
    const rows = [];

    data.items.forEach((element: any) => {
      const row = new ItemRow(
        element.id,
        element.name,
        element.brand.name,
        element.gender,
        element.retailPrice
      );

      rows.push(row);
    });

    return rows;
  }

  changePage(event?: PageEvent): PageEvent {
    this.itemsService.getItems(
      new ItemsParams(10, event.pageIndex + 1, this.category, this.searchText)
    );
    return event;
  }

  getPaginationData(items: PagedList<Item>): Pagination {
    return {
      pageIndex: items.pageIndex,
      hasNextPage: items.hasNextPage,
      hasPreviousPage: items.hasPreviousPage,
      totalPages: items.totalPages,
      totalCount: items.totalCount
    };
  }

  onCategoryChange(category: string): void {
    this.category = category;
    this.itemsService.getItems(
      new ItemsParams(10, 1, this.category, this.searchText)
    );
  }

  onSearch(searchText: string): void {
    this.searchText = searchText;
    this.itemsService.getItems(
      new ItemsParams(10, 1, this.category, this.searchText)
    );
  }

  ngOnDestroy(): void {
    this.itemsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
