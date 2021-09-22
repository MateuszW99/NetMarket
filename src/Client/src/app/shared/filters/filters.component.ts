import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ItemsParams } from '../items/items-params';
import { ItemsService } from '../items/items.service';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css']
})
export class FiltersComponent implements OnInit {
  constructor(private itemsService: ItemsService) {}

  @Input() category = 'sneakers';
  @Input() brands: string[] = ['Adidas', 'Jordan', 'Nike', 'Other'];
  priceRanges: string[] = [
    'Under $100',
    '$100 - $200',
    '$200 - $300',
    '$300 - $400',
    '$400 - $500',
    '$500 +'
  ];
  form: FormGroup;

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', Validators.maxLength(100)),
      make: new FormControl('', Validators.maxLength(100)),
      brand: new FormControl(''),
      model: new FormControl('', Validators.maxLength(100)),
      otherBrand: new FormControl('', Validators.maxLength(30)),
      price: new FormControl('')
    });
  }

  onSubmit(): void {
    const filters: ItemsParams = this.getFilters();

    console.log(filters);
    this.itemsService.getItems(filters);
  }

  onRemoveFilters(): void {
    this.form.reset();
    this.itemsService.getItems(new ItemsParams(15, 1, this.category));
  }

  private getFilters(): ItemsParams {
    let minPrice: number = null;
    let maxPrice: number = null;

    switch (this.form.value.price) {
      case 'Under $100': {
        minPrice = null;
        maxPrice = 100;
        break;
      }
      case '$100 - $200': {
        minPrice = 100;
        maxPrice = 200;
        break;
      }
      case '$200 - $300': {
        minPrice = 200;
        maxPrice = 300;
        break;
      }
      case '$300 - $400': {
        minPrice = 300;
        maxPrice = 400;
        break;
      }
      case '$400 - $500': {
        minPrice = 400;
        maxPrice = 500;
        break;
      }
      case '$500 +': {
        minPrice = 500;
        maxPrice = null;
        break;
      }
    }

    return new ItemsParams(
      15,
      1,
      this.category,
      this.form.value.name,
      this.form.value.make,
      this.form.value.brand === 'Other'
        ? this.form.value.otherBrand
        : this.form.value.brand,
      this.form.value.model,
      minPrice,
      maxPrice
    );
  }
}
