import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { ItemsService } from '../items.service';
import { ItemDetails } from './item-details.model';
import Modal from 'bootstrap/js/dist/modal';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent implements OnInit, OnDestroy {
  itemId = '';
  itemCard: ItemDetails;
  itemSubscription: Subscription;
  size: string;
  loading = true;
  error = false;

  constructor(
    private route: ActivatedRoute,
    private itemsService: ItemsService,
    private clipboardService: ClipboardService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.itemId = this.route.snapshot.params['id'];

    this.route.params.subscribe((params) => {
      this.itemSubscription = this.itemsService
        .getItemById(params['id'])
        .subscribe(
          (itemCard: ItemDetails) => {
            this.itemCard = itemCard;
            this.loading = false;
            this.setDefaultSize();
            this.error = false;
          },
          () => {
            this.error = true;
          }
        );
    });
  }

  onShare(): void {
    this.clipboardService.copyFromContent(
      window.location.origin + window.location.pathname
    );
    this.toastr.success('Link copied to clipboard');
  }

  onSelectSize(): void {
    const element = document.getElementById('sizeModal') as HTMLElement;
    const sizeModal = new Modal(element);
    sizeModal.show();
  }

  changeSize(size: string): void {
    this.size = size;
  }

  setDefaultSize(): void {
    if (this.itemCard.item.category === 'Sneakers') {
      this.size = '14';
    } else if (this.itemCard.item.category === 'Streetwear') {
      this.size = 'M';
    } else {
      this.size = 'noSize';
    }
  }

  ngOnDestroy(): void {
    this.itemSubscription.unsubscribe();
  }
}
