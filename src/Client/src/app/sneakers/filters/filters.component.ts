import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Filters } from './filters';

@Component({
  selector: 'app-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css']
})
export class FiltersComponent implements OnInit {
  form: FormGroup;
  filters: Filters = new Filters();

  ngOnInit(): void {
    this.filters.category = 'Sneakers';

    this.form = new FormGroup({
      name: new FormControl('', Validators.maxLength(100)),
      brand: new FormControl(''),
      model: new FormControl('', Validators.maxLength(100)),
      otherBrand: new FormControl('', Validators.maxLength(30)),
      price: new FormControl('')
    });
  }

  onSubmit(): void {
    this.getFormValues();
    console.log(this.filters);
    //TODO get items with applied filters
  }

  onRemoveFilters(): void {
    this.form.reset();
    //TODO reload items
  }

  private getFormValues(): void {
    this.filters.name = this.form.value.name;

    if (this.form.value.brand === 'other') {
      this.filters.brand = this.form.value.otherBrand;
    } else {
      this.filters.brand = this.form.value.brand;
    }

    this.filters.model = this.form.value.model;

    switch (this.form.value.price) {
      case '100': {
        this.filters.minPrice = null;
        this.filters.maxPrice = 100;
        break;
      }
      case '100-200': {
        this.filters.minPrice = 100;
        this.filters.maxPrice = 200;
        break;
      }
      case '200-300': {
        this.filters.minPrice = 200;
        this.filters.maxPrice = 300;
        break;
      }
      case '300-400': {
        this.filters.minPrice = 300;
        this.filters.maxPrice = 400;
        break;
      }
      case '400-500': {
        this.filters.minPrice = 400;
        this.filters.maxPrice = 500;
        break;
      }
      case '500+': {
        this.filters.minPrice = 500;
        this.filters.maxPrice = null;
        break;
      }
    }
  }
}
