import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
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
  itemCard: ItemDetails;
  itemSubscription: Subscription;
  size = 14;

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService,
    private clipboardService: ClipboardService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.itemId = this.route.snapshot.params['id'];
    console.log(this.itemId);

    this.route.params.subscribe((params) => {
      this.itemSubscription = this.itemsService
        .getItemById(params['id'])
        .subscribe((itemCard: ItemDetails) => {
          this.itemCard = itemCard;
          console.log(this.itemCard);
        });
    });
  }

  onShare(): void {
    this.clipboardService.copyFromContent(
      window.location.origin + window.location.pathname
    );
    this.toastr.success('Link copied to clipboard');
  }

  ngOnDestroy(): void {
    this.itemSubscription.unsubscribe();
  }
}
