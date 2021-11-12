import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { Statuses } from '../../statuses';
import { SupervisorService } from '../../supervisor.service';
import { UpdateTransactionStatus } from '../../update-transaction-status';

@Component({
  selector: 'app-manage-transaction',
  templateUrl: './manage-transaction.component.html',
  styleUrls: ['./manage-transaction.component.css']
})
export class ManageTransactionComponent implements OnInit {
  form: FormGroup;
  updateTransactionStatusSubscription: Subscription;
  error = false;
  transactionId: string;
  status: string;
  statuses: Map<string, string> = Statuses;

  constructor(
    private supervisorService: SupervisorService,
    @Inject(MAT_DIALOG_DATA)
    public data: { transactionId: string; status: string },
    private dialogRef: MatDialogRef<ManageTransactionComponent>
  ) {}

  ngOnInit(): void {
    this.transactionId = this.data.transactionId;
    this.status = this.data.status;

    this.form = new FormGroup({
      status: new FormControl(this.status, [Validators.required])
    });
  }

  onSubmit(): void {
    const updateTransactionStatus = new UpdateTransactionStatus(
      this.transactionId,
      this.form.value.status
    );

    this.updateTransactionStatusSubscription = this.supervisorService
      .updateTransactionStatus(updateTransactionStatus)
      .subscribe(
        () => {
          this.supervisorService.getAssignedTransactions(1);
          this.error = false;
          this.dialogRef.close();
        },
        () => {
          this.error = true;
        }
      );
  }
}
