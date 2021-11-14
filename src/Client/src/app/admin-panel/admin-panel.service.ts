import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../shared/api-paths';
import { PagedList } from '../shared/paged-list';
import { AddSupervisor } from './employees/add-supervisor';
import { Supervisor } from './employees/supervisor.model';

@Injectable({
  providedIn: 'root'
})
export class AdminPanelService {
  supervisorsChanged = new Subject<PagedList<Supervisor>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  constructor(private http: HttpClient) {}

  getSupervisors(pageIndex: number, searchText?: string): void {
    this.loading.next(true);

    let params = new HttpParams();
    params = params.append('pageIndex', pageIndex);
    params = params.append('pageSize', 10);
    if (searchText && searchText.trim() !== '') {
      params = params.append('searchText', searchText);
    }

    this.fetchSupervisors(params).subscribe(
      (response: HttpResponse<PagedList<Supervisor>>) => {
        const supervisors = new PagedList<Supervisor>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );
        this.supervisorsChanged.next(supervisors);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading supervisors');
        this.loading.next(false);
      }
    );
  }

  addSupervisor(addSupervisor: AddSupervisor): Observable<unknown> {
    return this.http.post<unknown>(
      environment.apiUrl + ApiPaths.AdminPanel + ApiPaths.Supervisors,
      addSupervisor
    );
  }

  deleteSupervisor(supervisorId: string): Observable<unknown> {
    return this.http.delete<unknown>(
      environment.apiUrl +
        ApiPaths.AdminPanel +
        ApiPaths.Supervisors +
        `/${supervisorId}`
    );
  }

  private fetchSupervisors(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Supervisor>>> {
    return this.http.get<PagedList<Supervisor>>(
      environment.apiUrl + ApiPaths.AdminPanel + ApiPaths.Supervisors,
      {
        observe: 'response',
        params: params
      }
    );
  }
}
