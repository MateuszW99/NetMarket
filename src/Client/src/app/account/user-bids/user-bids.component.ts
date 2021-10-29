import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Ask } from 'src/app/shared/ask.model';
import { Bid } from 'src/app/shared/bid.model';
import { BidsService } from 'src/app/shared/bids/bids.service';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { UserBidEditComponent } from './user-bid-edit/user-bid-edit.component';
import { TableRow } from './table-row';

@Component({
  selector: 'app-user-bids',
  templateUrl: './user-bids.component.html',
  styleUrls: ['./user-bids.component.css']
})
export class UserBidsComponent implements OnInit, OnDestroy {
  bidsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  bids: Bid[];
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
  dataSource = new MatTableDataSource<TableRow>();

  constructor(private bidsService: BidsService, public dialog: MatDialog) {}

  ngOnInit(): void {
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

    this.bidsSubscription = this.bidsService.userBidsChanged.subscribe(
      (bids: PagedList<Bid>) => {
        this.paginationData = this.getPaginationData(bids);
        this.dataSource = new MatTableDataSource<TableRow>(this.getRows(bids));
      }
    );
  }

  changePage(event?: PageEvent): PageEvent {
    this.bidsService.getUserBids(event.pageIndex + 1);
    return event;
  }

  getRows(data: PagedList<Bid> | PagedList<Ask>): TableRow[] {
    const rows = [];

    data.items.forEach((element: Ask | Bid) => {
      const row = new TableRow(
        element.id,
        element.item.name,
        element.price.toString(),
        element.size.value,
        element instanceof Ask
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

  getPaginationData(bids: PagedList<Bid>): Pagination {
    return {
      pageIndex: bids.pageIndex,
      hasNextPage: bids.hasNextPage,
      hasPreviousPage: bids.hasPreviousPage,
      totalPages: bids.totalPages,
      totalCount: bids.totalCount
    };
  }

  onEdit(row: TableRow): void {
    this.dialog.open(UserBidEditComponent, {
      width: '600px',
      data: { bidRow: row }
    });
  }

  ngOnDestroy(): void {
    this.bidsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
