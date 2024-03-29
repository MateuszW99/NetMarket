import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { AddItem } from 'src/app/admin-panel/products/add-product/add-item';
import { UpdateItem } from 'src/app/admin-panel/products/edit-product/update-item';
import { environment } from 'src/environments/environment';
import { ApiPaths } from '../api-paths';
import { PagedList } from '../paged-list';
import { ItemDetails } from './item-details/item-details.model';
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
  paramsChanged = new Subject<ItemsParams>();

  getItems(params: ItemsParams): void {
    this.loading.next(true);
    this.paramsChanged.next(params);
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

  getItemById(id: string): Observable<ItemDetails> {
    return this.http.get<ItemDetails>(
      environment.apiUrl + ApiPaths.Items + `/${id}`
    );
  }

  getTrendingItems(category: string, count: number): Observable<ItemDetails[]> {
    let params = new HttpParams();
    params = params.append('category', category);
    params = params.append('count', count);

    return this.http.get<ItemDetails[]>(
      environment.apiUrl + ApiPaths.Items + ApiPaths.Trending,
      {
        params: params
      }
    );
  }

  updateItem(updateItem: UpdateItem): Observable<unknown> {
    return this.http.put<unknown>(
      environment.apiUrl + ApiPaths.Items + `/${updateItem.id}`,
      updateItem
    );
  }

  deleteItem(itemId: string): Observable<unknown> {
    return this.http.delete<unknown>(
      environment.apiUrl + ApiPaths.Items + `/${itemId}`
    );
  }

  addItem(addItem: AddItem): Observable<unknown> {
    return this.http.post<unknown>(
      environment.apiUrl + ApiPaths.Items,
      addItem
    );
  }
}
