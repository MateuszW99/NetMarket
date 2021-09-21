import { HttpParams } from '@angular/common/http';

export interface Params {
  pageSize: number;
  pageNumber: number;

  getHttpParams(): HttpParams;
}
