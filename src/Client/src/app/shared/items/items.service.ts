import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../api-paths';
import { PagedList } from '../paged-list';
import { Item } from './item.model';
import { ItemsParams } from './items-params';

@Injectable({
  providedIn: 'root'
})
export class ItemsService {
  constructor(private http: HttpClient) {}

  itemsChanged = new Subject<PagedList<Item>>();
  errorCatched = new Subject<string>();
  loading = new Subject<boolean>();

  getItems(params: ItemsParams): void {
    this.loading.next(true);
    const httpParams: HttpParams = params.getHttpParams();

    this.fetchItems(httpParams).subscribe(
      (response: HttpResponse<PagedList<Item>>) => {
        const items = new PagedList<Item>(
          response.body.items,
          response.body.pageIndex,
          response.body.totalPages,
          response.body.totalCount,
          response.body.hasPreviousPage,
          response.body.hasNextPage
        );

        console.log(response);
        console.log(response.body);
        console.log(items);

        this.itemsChanged.next(items);
        this.loading.next(false);
      },
      () => {
        this.errorCatched.next('An eror occured while loading the items');
        this.loading.next(false);
      }
    );
  }

  private fetchItems(
    params: HttpParams
  ): Observable<HttpResponse<PagedList<Item>>> {
    return this.http.get<PagedList<Item>>(environment.apiUrl + ApiPaths.Items, {
      observe: 'response',
      params: params
    });
  }
}