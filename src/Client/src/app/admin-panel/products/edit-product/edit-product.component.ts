import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Brand } from 'src/app/shared/items/brand.model';
import { ItemDetails } from 'src/app/shared/items/item-details/item-details.model';
import { Item } from 'src/app/shared/items/item.model';
import { ItemsParams } from 'src/app/shared/items/items-params';
import { ItemsService } from 'src/app/shared/items/items.service';
import { UpdateItem } from './update-item';

@Component({
  selector: 'app-edit-product',
  templateUrl: './edit-product.component.html',
  styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit, OnDestroy {
  itemId: string;
  currentCategory: string;
  searchText: string;
  currentPage: number;
  item: Item;
  form: FormGroup;
  getItemSubscription: Subscription;
  updateItemSubscription: Subscription;
  deleteItemSubscription: Subscription;
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
      itemId: string;
      currentPage: number;
      currentCategory: string;
      searchText: string;
    },
    private dialogRef: MatDialogRef<EditProductComponent>
  ) {}

  ngOnInit(): void {
    this.loading = true;
    this.itemId = this.data.itemId;
    this.currentCategory = this.data.currentCategory;
    this.currentPage = this.data.currentPage;
    this.searchText = this.data.searchText;
    this.getItemSubscription = this.itemsService
      .getItemById(this.itemId)
      .subscribe((item: ItemDetails) => {
        this.item = item.item;
        this.createForm();
        this.loading = false;
      });
  }

  onSubmit(): void {
    const updateItem = new UpdateItem(
      this.item.id,
      this.form.value.name,
      this.form.value.make,
      this.form.value.model,
      this.form.value.gender,
      this.form.value.retailPrice,
      this.form.value.description,
      this.form.value.imageUrl,
      new Brand(this.item.brand.id, this.form.value.brand),
      this.form.value.category
    );

    this.updateItemSubscription = this.itemsService
      .updateItem(updateItem)
      .subscribe(
        () => {
          this.itemsService.getItems(
            new ItemsParams(
              10,
              this.currentPage,
              this.currentCategory,
              this.searchText
            )
          );
          this.dialogRef.close();
        },
        () => {
          this.error = true;
        }
      );
  }

  onDelete() {
    this.deleteItemSubscription = this.itemsService
      .deleteItem(this.itemId)
      .subscribe(() => {
        this.itemsService.getItems(
          new ItemsParams(
            10,
            this.currentPage,
            this.currentCategory,
            this.searchText
          )
        );
        this.dialogRef.close();
      });
  }

  private createForm(): void {
    this.form = new FormGroup({
      name: new FormControl(this.item.name, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      make: new FormControl(this.item.make, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      model: new FormControl(this.item.model, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      gender: new FormControl(this.item.gender, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(10)
      ]),
      retailPrice: new FormControl(this.item.retailPrice.toFixed(2), [
        Validators.required,
        Validators.min(0.01),
        Validators.minLength(1),
        Validators.maxLength(50),
        Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')
      ]),
      description: new FormControl(this.item.description, [
        Validators.maxLength(1500)
      ]),
      imageUrl: new FormControl(this.item.imageUrl, [
        Validators.required,
        Validators.pattern(
          /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/
        )
      ]),
      brand: new FormControl(this.item.brand.name, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ]),
      category: new FormControl(this.item.category, [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(150)
      ])
    });
  }

  ngOnDestroy(): void {
    this.getItemSubscription.unsubscribe();

    if (this.updateItemSubscription) {
      this.updateItemSubscription.unsubscribe();
    }
    if (this.deleteItemSubscription) {
      this.deleteItemSubscription.unsubscribe();
    }
  }
}
