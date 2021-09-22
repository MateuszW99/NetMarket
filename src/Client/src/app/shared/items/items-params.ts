import { HttpParams } from '@angular/common/http';
import { Params } from 'src/app/shared/params';

export class ItemsParams implements Params {
  constructor(
    public pageSize: number = 15,
    public pageNumber: number = 1,
    public category: string,
    public name?: string,
    public make?: string,
    public brand?: string,
    public model?: string,
    public minPrice?: number,
    public maxPrice?: number
  ) {}

  getHttpParams(): HttpParams {
    let params = new HttpParams();
    params = params.append('pageNumber', this.pageNumber);
    params = params.append('pageSize', this.pageSize);
    params = params.append('category', this.category);

    if (this.name && this.name.trim() !== '') {
      params = params.append('name', this.name);
    }
    if (this.make && this.make.trim() !== '') {
      params = params.append('make', this.make);
    }
    if (this.brand && this.brand.trim() !== '') {
      params = params.append('brand', this.brand);
    }
    if (this.model && this.model.trim() !== '') {
      params = params.append('model', this.model);
    }
    if (this.minPrice) {
      params = params.append('minPrice', this.minPrice);
    }
    if (this.maxPrice) {
      params = params.append('maxPrice', this.maxPrice);
    }

    return params;
  }
}
