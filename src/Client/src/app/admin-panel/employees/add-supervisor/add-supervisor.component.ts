import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
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
  error = '';
  loading = false;

  constructor(
    private adminPanelService: AdminPanelService,
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
          this.error = '';
          this.adminPanelService.getSupervisors(1);
          this.dialogRef.close();
        },
        (error) => {
          this.loading = false;
          if (
            !error.error.errorMessages ||
            error.error.errorMessages.length === 0
          ) {
            this.error = 'Something went wrong';
          } else {
            this.error = error.error.errorMessages[0];
          }
        }
      );
  }

  private createForm(): void {
    this.form = new FormGroup({
      username: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(30)
      ]),
      email: new FormControl('', [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(256),
        Validators.email
      ]),
      password: new FormControl('', [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(64)
      ])
    });
  }

  ngOnDestroy(): void {
    if (this.addSupervisorSubscription) {
      this.addSupervisorSubscription.unsubscribe();
    }
  }
}
