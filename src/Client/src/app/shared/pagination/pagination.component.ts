import { Component, Input } from '@angular/core';
import { ItemsParams } from '../items/items-params';
import { ItemsService } from '../items/items.service';
import { Pagination } from '../pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent {
  @Input() paginationData: Pagination;
  @Input() params: ItemsParams;

  constructor(private itemsService: ItemsService) {}

  onChangePage(pageIndex: number): void {
    this.params.pageIndex = pageIndex;
    this.itemsService.getItems(this.params);
  }

  onNextPage(): void {
    this.params.pageIndex++;
    this.itemsService.getItems(this.params);
  }

  onPreviousPage(): void {
    this.params.pageIndex--;
    this.itemsService.getItems(this.params);
  }
}
