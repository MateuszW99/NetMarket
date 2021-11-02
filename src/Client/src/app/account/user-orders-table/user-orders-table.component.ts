import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Ask } from 'src/app/shared/ask.model';
import { AsksService } from 'src/app/shared/asks/asks.service';
import { Bid } from 'src/app/shared/bid.model';
import { BidsService } from 'src/app/shared/bids/bids.service';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { TableRow } from './table-row';
import { OrderEditComponent } from './order-edit/order-edit.component';

@Component({
  selector: 'app-user-orders-table',
  templateUrl: './user-orders-table.component.html',
  styleUrls: ['./user-orders-table.component.css']
})
export class UserOrdersTableComponent implements OnInit, OnDestroy {
  @Input() type: string; //asks or bids
  dataSource = new MatTableDataSource<TableRow>();
  ordersSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  paginationData: Pagination;
  pageEvent: PageEvent;
  displayedColumns = [
    'id',
    'name',
    'price',
    'fee',
    'size',
    'lowestAsk',
    'highestBid',
    'expires'
  ];
  constructor(
    public dialog: MatDialog,
    private asksService: AsksService,
    private bidsService: BidsService
  ) {}

  ngOnInit(): void {
    if (this.type === 'asks') {
      this.asksTable();
    } else {
      this.bidsTable();
    }
  }

  onEdit(row: TableRow): void {
    this.dialog.open(OrderEditComponent, {
      width: '600px',
      data: { type: this.type, row: row }
    });
  }

  getRows(data: PagedList<Ask | Bid>): TableRow[] {
    const rows = [];

    data.items.forEach((element: any) => {
      const row = new TableRow(
        element.id,
        element.item.name,
        element.price.toString(),
        element.size.value,
        this.type === 'asks'
          ? element.sellerFee.toString()
          : element.buyerFee.toString(),
        element.item.lowestAsk === null
          ? 'No asks'
          : element.item.lowestAsk.toString(),
        element.item.highestBid === null
          ? 'No bids'
          : element.item.highestBid.toString(),
        element.expires.toString()
      );

      rows.push(row);
    });

    return rows;
  }

  bidsTable(): void {
    this.bidsService.getUserBids(1);
    this.loadingSubscription = this.bidsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.bidsService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.ordersSubscription = this.bidsService.userBidsChanged.subscribe(
      (bids: PagedList<Bid>) => {
        this.dataSource = new MatTableDataSource<TableRow>(this.getRows(bids));
        this.paginationData = this.getPaginationData(bids);
      }
    );
  }

  asksTable(): void {
    this.asksService.getUserAsks(1);
    this.loadingSubscription = this.asksService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.asksService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.ordersSubscription = this.asksService.userAsksChanged.subscribe(
      (asks: PagedList<Ask>) => {
        this.dataSource = new MatTableDataSource<TableRow>(this.getRows(asks));
        this.paginationData = this.getPaginationData(asks);
      }
    );
  }

  changePage(event?: PageEvent): PageEvent {
    if (this.type === 'asks') {
      this.asksService.getUserAsks(event.pageIndex + 1);
    } else {
      this.bidsService.getUserBids(event.pageIndex + 1);
    }
    return event;
  }

  getPaginationData(orders: PagedList<Ask | Bid>): Pagination {
    return {
      pageIndex: orders.pageIndex,
      hasNextPage: orders.hasNextPage,
      hasPreviousPage: orders.hasPreviousPage,
      totalPages: orders.totalPages,
      totalCount: orders.totalCount
    };
  }

  ngOnDestroy(): void {
    this.ordersSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
