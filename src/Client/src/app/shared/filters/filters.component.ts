import { Component, HostListener, Input, OnInit } from '@angular/core';
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

  @HostListener('window:resize', ['$event']) onResize(event: {
    target: { innerWidth: number };
  }): void {
    if (event.target.innerWidth > 767) {
      this.mobile = false;
    } else {
      this.mobile = true;
    }
  }

  @Input() category = '';
  @Input() brands: string[];
  @Input() genders: string[]; //sneakers and streetwear only

  priceRanges: string[] = [
    'All prices',
    'Under $100',
    '$100 - $200',
    '$200 - $300',
    '$300 - $400',
    '$400 - $500',
    '$500 +'
  ];
  form: FormGroup;
  mobile = false;
  otherBrandSelected = false;

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl('', Validators.maxLength(100)),
      make: new FormControl('', Validators.maxLength(100)),
      brand: new FormControl(this.brands[0]),
      otherBrand: new FormControl('', Validators.maxLength(30)),
      model: new FormControl('', Validators.maxLength(100)),
      gender: new FormControl(this.genders[0]),
      price: new FormControl(this.priceRanges[0])
    });

    this.form.get('brand').valueChanges.subscribe((selectedBrand) => {
      this.form.controls.otherBrand.reset();
      this.otherBrandSelected = selectedBrand === 'Other';
    });
  }

  onSubmit(): void {
    const filters: ItemsParams = this.getFilters();
    this.itemsService.getItems(filters);
  }

  onRemoveFilters(): void {
    this.form.reset({
      brand: this.brands[0],
      gender: this.genders[0],
      price: this.priceRanges[0]
    });
    this.itemsService.getItems(new ItemsParams(15, 1, this.category));
  }

  private getFilters(): ItemsParams {
    let minPrice: number = null;
    let maxPrice: number = null;

    switch (this.form.value.price) {
      case 'All prices': {
        minPrice = null;
        maxPrice = null;
        break;
      }
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

    const selectedBrand = this.form.value.brand;
    const brand = selectedBrand === 'All' ? null : selectedBrand;

    const selectedGender = this.form.value.gender;
    const gender = selectedGender === 'All' ? null : selectedGender;

    return new ItemsParams(
      15,
      1,
      this.category,
      this.form.value.name,
      this.form.value.make,
      gender,
      brand === 'Other' || this.brands.length === 0
        ? this.form.value.otherBrand
        : brand,
      this.form.value.model,
      minPrice,
      maxPrice
    );
  }
}
