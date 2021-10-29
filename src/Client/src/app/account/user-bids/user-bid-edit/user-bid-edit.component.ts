import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { BidsService } from 'src/app/shared/bids/bids.service';
import { UpdateBid } from 'src/app/shared/bids/update-bid.model';
import { SneakersSizes } from 'src/app/shared/size-modal/sneakers-sizes';
import { StreetwearSizes } from 'src/app/shared/size-modal/streetwear-sizes';
import { TableRow } from '../table-row';

@Component({
  selector: 'app-user-bid-edit',
  templateUrl: './user-bid-edit.component.html',
  styleUrls: ['./user-bid-edit.component.css']
})
export class UserBidEditComponent implements OnInit, OnDestroy {
  bidRow: TableRow;
  nosize = false;
  allowedSizes: string[] = [];
  form: FormGroup;
  error = false;
  deleted = false;
  updateBidSubscription: Subscription;
  deleteBidSubscription: Subscription;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { bidRow: TableRow },
    private dialogRef: MatDialogRef<UserBidEditComponent>,
    private bidsService: BidsService
  ) {}

  ngOnInit(): void {
    this.bidRow = this.data.bidRow;
    this.checkSize(this.bidRow.size);
    this.form = new FormGroup({
      size: new FormControl(this.bidRow.size, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(50)
      ]),
      price: new FormControl(this.bidRow.price, [
        Validators.required,
        Validators.min(0.01),
        Validators.minLength(1),
        Validators.maxLength(50)
      ])
    });
  }

  onSubmit(): void {
    const bid: UpdateBid = new UpdateBid(
      this.bidRow.id,
      this.nosize === true ? 'nosize' : this.form.value.size,
      this.form.value.price
    );
    this.updateBidSubscription = this.bidsService
      .updateBid(bid)
      .subscribe(() => {
        this.bidsService.getUserBids(1);
      });

    this.dialogRef.close();
  }

  onDelete(): void {
    this.deleteBidSubscription = this.bidsService
      .deleteBid(this.bidRow.id)
      .subscribe(() => {
        this.deleted = true;
        this.bidsService.getUserBids(1);
      });
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
    if (this.updateBidSubscription) {
      this.updateBidSubscription.unsubscribe();
    }
    if (this.deleteBidSubscription) {
      this.deleteBidSubscription.unsubscribe();
    }
  }
}
