import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { Ask } from 'src/app/shared/ask.model';
import { AsksService } from 'src/app/shared/asks/asks.service';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { TableRow } from '../user-bids/table-row';
import { UserAskEditComponent } from './user-ask-edit/user-ask-edit.component';

@Component({
  selector: 'app-user-asks',
  templateUrl: './user-asks.component.html',
  styleUrls: ['./user-asks.component.css']
})
export class UserAsksComponent implements OnInit {
  asksSubscription: Subscription;
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
  dataSource = new MatTableDataSource<TableRow>();

  constructor(private asksService: AsksService, public dialog: MatDialog) {}

  ngOnInit(): void {
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

    this.asksSubscription = this.asksService.userAsksChanged.subscribe(
      (asks: PagedList<Ask>) => {
        this.paginationData = this.getPaginationData(asks);
        this.dataSource = new MatTableDataSource<TableRow>(this.getRows(asks));
      }
    );
  }

  changePage(event?: PageEvent): PageEvent {
    this.asksService.getUserAsks(event.pageIndex + 1);
    return event;
  }

  getRows(data: PagedList<Ask>): TableRow[] {
    const rows = [];

    data.items.forEach((element: Ask) => {
      const row = new TableRow(
        element.id,
        element.item.name,
        element.price.toString(),
        element.size.value,

        element.sellerFee.toString(),
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

  getPaginationData(asks: PagedList<Ask>): Pagination {
    return {
      pageIndex: asks.pageIndex,
      hasNextPage: asks.hasNextPage,
      hasPreviousPage: asks.hasPreviousPage,
      totalPages: asks.totalPages,
      totalCount: asks.totalCount
    };
  }

  onEdit(row: TableRow): void {
    this.dialog.open(UserAskEditComponent, {
      width: '600px',
      data: { askRow: row }
    });
  }

  ngOnDestroy(): void {
    this.asksSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
