import { Pagination } from './pagination';

export class PagedList<T> implements Pagination {
  constructor(
    public items: T[] = [],
    public pageIndex: number,
    public totalPages: number,
    public totalCount: number,
    public hasPreviousPage: boolean,
    public hasNextPage: boolean
  ) {}
}
