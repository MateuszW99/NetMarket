import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { TableRow } from 'src/app/account/user-orders-table/table-row';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { Statuses } from '../../statuses';
import { ManageTransactionComponent } from '../manage-transaction/manage-transaction.component';
import { SupervisorService } from '../../supervisor.service';
import { Transaction } from '../../transaction';
import { TransactionRow } from './transaction-row';

@Component({
  selector: 'app-supervisor-transactions',
  templateUrl: './supervisor-transactions.component.html',
  styleUrls: ['./supervisor-transactions.component.css']
})
export class SupervisorTransactionsComponent implements OnInit {
  status = '';
  dataSource = new MatTableDataSource<TransactionRow>();
  transactionsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  paginationData: Pagination;
  pageEvent: PageEvent;
  statuses: Map<string, string> = new Map(Statuses);
  displayedColumns = [
    'id',
    'category',
    'name',
    'companyProfit',
    'date',
    'status'
  ];
  form: FormGroup;

  constructor(
    public dialog: MatDialog,
    private supervisorService: SupervisorService
  ) {}

  ngOnInit(): void {
    this.statuses.set('All', 'All');

    this.form = new FormGroup({
      status: new FormControl('All')
    });

    this.supervisorService.getAssignedTransactions(1);
    this.loadingSubscription = this.supervisorService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.supervisorService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.transactionsSubscription =
      this.supervisorService.transactionsChanged.subscribe(
        (transactions: PagedList<Transaction>) => {
          this.dataSource = new MatTableDataSource<TransactionRow>(
            this.getRows(transactions)
          );
          this.paginationData = this.getPaginationData(transactions);
        }
      );
  }

  onManage(row: TransactionRow): void {
    this.dialog.open(ManageTransactionComponent, {
      width: '600px',
      data: { transactionId: row.id, status: row.status }
    });
  }

  getRows(data: PagedList<Transaction>): TransactionRow[] {
    const rows = [];

    data.items.forEach((element: any) => {
      const row = new TransactionRow(
        element.id,
        element.ask.item.category,
        element.ask.item.name,
        element.companyProfit,
        element.startDate,
        element.status
      );

      rows.push(row);
    });

    return rows;
  }

  changePage(event?: PageEvent): PageEvent {
    this.supervisorService.getAssignedTransactions(event.pageIndex + 1);

    return event;
  }

  getPaginationData(transactions: PagedList<Transaction>): Pagination {
    return {
      pageIndex: transactions.pageIndex,
      hasNextPage: transactions.hasNextPage,
      hasPreviousPage: transactions.hasPreviousPage,
      totalPages: transactions.totalPages,
      totalCount: transactions.totalCount
    };
  }

  onFilterByStatus() {
    const status = this.form.value.status;

    this.supervisorService.getAssignedTransactions(
      1,
      status !== 'All' ? status : ''
    );
  }

  ngOnDestroy(): void {
    this.transactionsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }

  asIsOrder(a, b) {
    return 1;
  }
}
