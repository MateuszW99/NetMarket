import { HttpParams } from '@angular/common/http';

export interface Params {
  pageSize: number;
  pageIndex: number;

  getHttpParams(): HttpParams;
}
