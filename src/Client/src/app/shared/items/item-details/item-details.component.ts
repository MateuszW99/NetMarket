import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Item } from '../item.model';
import { ItemsService } from '../items.service';
import { ItemDetails } from './item-details.model';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent implements OnInit, OnDestroy {
  itemId = '';
  item: ItemDetails;
  itemSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService
  ) {}

  ngOnInit(): void {
    this.itemId = this.route.snapshot.params['id'];
    console.log(this.itemId);

    this.route.params.subscribe((params) => {
      this.itemSubscription = this.itemsService
        .getItemById(params['id'])
        .subscribe((item: ItemDetails) => {
          console.log(item);
          this.item = item;
          console.log(this.item);
        });
    });
  }

  ngOnDestroy(): void {
    this.itemSubscription.unsubscribe();
  }
}
