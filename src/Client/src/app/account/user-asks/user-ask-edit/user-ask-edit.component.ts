import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AsksService } from 'src/app/shared/asks/asks.service';
import { UpdateAsk } from 'src/app/shared/asks/update-ask.model';
import { SneakersSizes } from 'src/app/shared/size-modal/sneakers-sizes';
import { StreetwearSizes } from 'src/app/shared/size-modal/streetwear-sizes';
import { TableRow } from '../../user-bids/table-row';

@Component({
  selector: 'app-user-ask-edit',
  templateUrl: './user-ask-edit.component.html',
  styleUrls: ['./user-ask-edit.component.css']
})
export class UserAskEditComponent implements OnInit {
  askRow: TableRow;
  nosize = false;
  allowedSizes: string[] = [];
  form: FormGroup;
  error = false;
  deleted = false;
  updateAskSubscription: Subscription;
  deleteAskSubscription: Subscription;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { askRow: TableRow },
    private dialogRef: MatDialogRef<UserAskEditComponent>,
    private asksService: AsksService
  ) {}

  ngOnInit(): void {
    this.askRow = this.data.askRow;
    this.checkSize(this.askRow.size);
    this.form = new FormGroup({
      size: new FormControl(this.askRow.size, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(50)
      ]),
      price: new FormControl(this.askRow.price, [
        Validators.required,
        Validators.min(0.01),
        Validators.minLength(1),
        Validators.maxLength(50)
      ])
    });
  }

  onSubmit(): void {
    const ask: UpdateAsk = new UpdateAsk(
      this.askRow.id,
      this.nosize === true ? 'nosize' : this.form.value.size,
      this.form.value.price
    );
    this.updateAskSubscription = this.asksService
      .updateAsk(ask)
      .subscribe(() => {
        this.asksService.getUserAsks(1);
      });

    this.dialogRef.close();
  }

  onDelete(): void {
    this.deleteAskSubscription = this.asksService
      .deleteAsk(this.askRow.id)
      .subscribe(() => {
        this.deleted = true;
        this.asksService.getUserAsks(1);
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
    if (this.updateAskSubscription) {
      this.updateAskSubscription.unsubscribe();
    }
    if (this.deleteAskSubscription) {
      this.deleteAskSubscription.unsubscribe();
    }
  }
}
