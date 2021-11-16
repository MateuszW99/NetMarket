import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AdminPanelService } from '../../admin-panel.service';

@Component({
  selector: 'app-delete-supervisor',
  templateUrl: './delete-supervisor.component.html',
  styleUrls: ['./delete-supervisor.component.css']
})
export class DeleteSupervisorComponent implements OnInit, OnDestroy {
  deleteSupervisorSubscription: Subscription;
  supervisorId: string;

  constructor(
    private adminPanelService: AdminPanelService,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      supervisorId: string;
    },
    private dialogRef: MatDialogRef<DeleteSupervisorComponent>
  ) {}

  ngOnInit(): void {
    this.supervisorId = this.data.supervisorId;
  }

  onDelete() {
    this.deleteSupervisorSubscription = this.adminPanelService
      .deleteSupervisor(this.supervisorId)
      .subscribe(() => {
        this.adminPanelService.getSupervisors(1);
        this.dialogRef.close();
      });
  }

  ngOnDestroy(): void {
    if (this.deleteSupervisorSubscription) {
      this.deleteSupervisorSubscription.unsubscribe();
    }
  }
}
