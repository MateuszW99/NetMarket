import { Component, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { TransactionService } from 'src/app/shared/services/orders/transaction.service';
import { Statuses } from 'src/app/supervisor-panel/statuses';
import { Transaction } from 'src/app/supervisor-panel/transaction';
import { UserTransactionRow } from './user-transaction-row';

@Component({
  selector: 'app-transactions-table',
  templateUrl: './transactions-table.component.html',
  styleUrls: ['./transactions-table.component.css']
})
export class TransactionsTableComponent implements OnInit {
  searchText = '';
  currentPage: number;
  dataSource = new MatTableDataSource<UserTransactionRow>();
  userTransactionsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  paginationData: Pagination;
  pageEvent: PageEvent;
  statuses: Map<string, string> = new Map(Statuses);
  displayedColumns = [
    'name',
    'category',
    'startDate',
    'totalBuyerCost',
    'status'
  ];

  constructor(private transactionsService: TransactionService) {}

  ngOnInit(): void {
    this.transactionsService.getUserTransactions(1, this.searchText);
    this.loadingSubscription = this.transactionsService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.transactionsService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.userTransactionsSubscription =
      this.transactionsService.userTransactionsChanged.subscribe(
        (transactions: PagedList<Transaction>) => {
          this.dataSource = new MatTableDataSource<UserTransactionRow>(
            this.getRows(transactions)
          );
          this.paginationData = this.getPaginationData(transactions);
        }
      );
  }

  getRows(data: PagedList<Transaction>): UserTransactionRow[] {
    const rows = [];

    data.items.forEach((element: any) => {
      const row = new UserTransactionRow(
        element.ask.item.name,
        element.ask.item.category,
        element.startDate,
        element.totalBuyerCost,
        element.status
      );

      rows.push(row);
    });

    return rows;
  }

  changePage(event?: PageEvent): PageEvent {
    this.transactionsService.getUserTransactions(
      event.pageIndex + 1,
      this.searchText
    );
    this.currentPage = event.pageIndex + 1;
    return event;
  }

  getPaginationData(items: PagedList<Transaction>): Pagination {
    this.currentPage = items.pageIndex;
    return {
      pageIndex: items.pageIndex,
      hasNextPage: items.hasNextPage,
      hasPreviousPage: items.hasPreviousPage,
      totalPages: items.totalPages,
      totalCount: items.totalCount
    };
  }

  onSearch(searchText: string): void {
    this.searchText = searchText;
    this.transactionsService.getUserTransactions(1, this.searchText);
  }

  ngOnDestroy(): void {
    this.userTransactionsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
