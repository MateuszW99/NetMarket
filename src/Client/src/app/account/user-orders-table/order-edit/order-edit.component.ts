import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AsksService } from 'src/app/shared/asks/asks.service';
import { BidsService } from 'src/app/shared/bids/bids.service';
import { SneakersSizes } from 'src/app/shared/size-modal/sneakers-sizes';
import { StreetwearSizes } from 'src/app/shared/size-modal/streetwear-sizes';
import { TableRow } from '../table-row';
import { UpdateOrder } from './update-order';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {
  type: string;
  row: TableRow;
  nosize = false;
  allowedSizes: string[] = [];
  form: FormGroup;
  error = false;
  deleted = false;
  updateSubscription: Subscription;
  deleteSubscription: Subscription;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { type: string; row: TableRow },
    private dialogRef: MatDialogRef<OrderEditComponent>,
    private asksService: AsksService,
    private bidsService: BidsService
  ) {}

  ngOnInit(): void {
    this.type = this.data.type;
    this.row = this.data.row;
    this.checkSize(this.row.size);
    this.form = new FormGroup({
      size: new FormControl(this.row.size, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(50)
      ]),
      price: new FormControl(this.row.price, [
        Validators.required,
        Validators.min(0.01),
        Validators.minLength(1),
        Validators.maxLength(50)
      ])
    });
  }

  onSubmit(): void {
    const order: UpdateOrder = new UpdateOrder(
      this.row.id,
      this.nosize === true ? 'nosize' : this.form.value.size,
      this.form.value.price
    );

    if (this.type === 'asks') {
      this.updateSubscription = this.asksService
        .updateAsk(order)
        .subscribe(() => {
          this.asksService.getUserAsks(1);
        });
    } else {
      this.updateSubscription = this.bidsService
        .updateBid(order)
        .subscribe(() => {
          this.bidsService.getUserBids(1);
        });
    }

    this.dialogRef.close();
  }

  onDelete(): void {
    if (this.type === 'asks') {
      this.deleteSubscription = this.asksService
        .deleteAsk(this.row.id)
        .subscribe(() => {
          this.deleted = true;
          this.asksService.getUserAsks(1);
        });
    } else {
      this.deleteSubscription = this.bidsService
        .deleteBid(this.row.id)
        .subscribe(() => {
          this.deleted = true;
          this.bidsService.getUserBids(1);
        });
    }
  }

  checkSize(size: string): void {
    if (StreetwearSizes.includes(size)) {
      this.allowedSizes = StreetwearSizes;
    } else if (SneakersSizes.includes(size)) {
      this.allowedSizes = SneakersSizes;
    } else {
      this.nosize = true;
    }
  }

  ngOnDestroy(): void {
    if (this.updateSubscription) {
      this.updateSubscription.unsubscribe();
    }
    if (this.deleteSubscription) {
      this.deleteSubscription.unsubscribe();
    }
  }
}
