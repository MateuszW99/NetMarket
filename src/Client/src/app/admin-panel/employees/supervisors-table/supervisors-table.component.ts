import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { PagedList } from 'src/app/shared/paged-list';
import { Pagination } from 'src/app/shared/pagination';
import { AdminPanelService } from '../../admin-panel.service';
import { DeleteSupervisorComponent } from '../delete-supervisor/delete-supervisor.component';
import { Supervisor } from '../supervisor.model';
import { SupervisorRow } from './supervisor-row';

@Component({
  selector: 'app-supervisors-table',
  templateUrl: './supervisors-table.component.html',
  styleUrls: ['./supervisors-table.component.css']
})
export class SupervisorsTableComponent implements OnInit, OnDestroy {
  searchText = '';
  currentPage: number;
  dataSource = new MatTableDataSource<SupervisorRow>();
  supervisorsSubscription: Subscription;
  loadingSubscription: Subscription;
  errrorSubscription: Subscription;
  loading = true;
  error: string;
  paginationData: Pagination;
  pageEvent: PageEvent;
  displayedColumns = ['id', 'username', 'email'];

  constructor(
    public dialog: MatDialog,
    private adminPanelService: AdminPanelService
  ) {}

  ngOnInit(): void {
    this.adminPanelService.getSupervisors(1, this.searchText);
    this.loadingSubscription = this.adminPanelService.loading.subscribe(
      (isLoading) => {
        this.loading = isLoading;
      }
    );

    this.errrorSubscription = this.adminPanelService.errorCatched.subscribe(
      (error) => {
        this.error = error;
      }
    );
    this.supervisorsSubscription =
      this.adminPanelService.supervisorsChanged.subscribe(
        (supervisors: PagedList<Supervisor>) => {
          this.dataSource = new MatTableDataSource<SupervisorRow>(
            this.getRows(supervisors)
          );
          this.paginationData = this.getPaginationData(supervisors);
        }
      );
  }

  onDelete(row: SupervisorRow): void {
    this.dialog.open(DeleteSupervisorComponent, {
      width: '600px',
      data: {
        supervisorId: row.id
      }
    });
  }

  getRows(data: PagedList<Supervisor>): SupervisorRow[] {
    const rows = [];

    data.items.forEach((element: any) => {
      const row = new SupervisorRow(
        element.id,
        element.username,
        element.email
      );

      rows.push(row);
    });

    return rows;
  }

  changePage(event?: PageEvent): PageEvent {
    this.adminPanelService.getSupervisors(event.pageIndex + 1, this.searchText);
    this.currentPage = event.pageIndex + 1;
    return event;
  }

  getPaginationData(items: PagedList<Supervisor>): Pagination {
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
    this.adminPanelService.getSupervisors(1, this.searchText);
  }

  ngOnDestroy(): void {
    this.supervisorsSubscription.unsubscribe();
    this.loadingSubscription.unsubscribe();
    this.errrorSubscription.unsubscribe();
  }
}
