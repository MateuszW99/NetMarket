import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { AdminPanelService } from '../../admin-panel.service';
import { AddSupervisor } from './add-supervisor';

@Component({
  selector: 'app-add-supervisor',
  templateUrl: './add-supervisor.component.html',
  styleUrls: ['./add-supervisor.component.css']
})
export class AddSupervisorComponent implements OnInit, OnDestroy {
  form: FormGroup;
  addSupervisorSubscription: Subscription;
  error = false;
  loading = false;

  constructor(
    private adminPanelService: AdminPanelService,
    @Inject(MAT_DIALOG_DATA)
    private dialogRef: MatDialogRef<AddSupervisorComponent>
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  onSubmit(): void {
    this.loading = true;

    const addSupervisor = new AddSupervisor(
      this.form.value.username,
      this.form.value.email,
      this.form.value.password
    );

    this.addSupervisorSubscription = this.adminPanelService
      .addSupervisor(addSupervisor)
      .subscribe(
        () => {
          this.loading = false;
          this.adminPanelService.getSupervisors(1);
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
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(50)
      ])
    });
  }

  ngOnDestroy(): void {
    if (this.addSupervisorSubscription) {
      this.addSupervisorSubscription.unsubscribe();
    }
  }
}
