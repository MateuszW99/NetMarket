import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-product-filters',
  templateUrl: './product-filters.component.html',
  styleUrls: ['./product-filters.component.css']
})
export class ProductFiltersComponent implements OnInit {
  form: FormGroup;
  categories = ['sneakers', 'streetwear', 'electronics', 'collectibles'];
  currentCategory = this.categories[0];
  @Output() categoryChanged = new EventEmitter<string>();
  @Output() searchChanged = new EventEmitter<string>();

  constructor() {}

  ngOnInit(): void {
    this.form = new FormGroup({
      search: new FormControl('')
    });
  }

  onSearch() {
    this.searchChanged.emit(this.form.value.search);
  }

  onCategoryChange(category: string) {
    this.currentCategory = category;
    this.categoryChanged.emit(category);
  }
}
