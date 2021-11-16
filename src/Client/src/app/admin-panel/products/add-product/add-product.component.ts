import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { ItemsParams } from 'src/app/shared/items/items-params';
import { ItemsService } from 'src/app/shared/items/items.service';
import { EditProductComponent } from '../edit-product/edit-product.component';
import { AddItem } from './add-item';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit, OnDestroy {
  currentCategory: string;
  form: FormGroup;
  addItemSubscription: Subscription;
  error = false;
  loading = false;
  genders: string[] = [
    'men',
    'women',
    'kids',
    'child',
    'preschool',
    'infant',
    'toddler',
    'unisex'
  ];
  categories: string[] = [
    'Sneakers',
    'Streetwear',
    'Electronics',
    'Collectibles'
  ];

  constructor(
    private itemsService: ItemsService,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      currentCategory: string;
    },
    private dialogRef: MatDialogRef<EditProductComponent>
  ) {}

  ngOnInit(): void {
    this.currentCategory = this.data.currentCategory;
    this.createForm();
  }

  onSubmit(): void {
    this.loading = true;

    const addItem = new AddItem(
      this.form.value.name,
      this.form.value.make,
      this.form.value.model,
      this.form.value.gender,
      this.form.value.retailPrice,
      this.form.value.description,
      this.form.value.imageUrl,
      this.form.value.smallImageUrl,
      this.form.value.thumbUrl,
      this.form.value.brand,
      this.form.value.category
    );

    this.addItemSubscription = this.itemsService.addItem(addItem).subscribe(
      () => {
        this.loading = false;
        this.itemsService.getItems(
          new ItemsParams(10, 1, this.currentCategory)
        );
        this.dialogRef.close();
      },
      () => {
        this.error = true;
        this.loading = false;
      }
    );
  }

  private createForm(): void {
    this.form = new FormGroup({
      name: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      make: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      model: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      gender: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(10)
      ]),
      retailPrice: new FormControl('', [
        Validators.required,
        Validators.min(0.01),
        Validators.minLength(1),
        Validators.maxLength(50),
        Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')
      ]),
      description: new FormControl('', [Validators.maxLength(1500)]),
      imageUrl: new FormControl('', [
        Validators.required,
        Validators.pattern(
          /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/
        )
      ]),
      smallImageUrl: new FormControl('', [
        Validators.required,
        Validators.pattern(
          /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/
        )
      ]),
      thumbUrl: new FormControl('', [
        Validators.required,
        Validators.pattern(
          /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/
        )
      ]),
      brand: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      category: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ])
    });
  }

  ngOnDestroy(): void {
    if (this.addItemSubscription) {
      this.addItemSubscription.unsubscribe();
    }
  }
}
