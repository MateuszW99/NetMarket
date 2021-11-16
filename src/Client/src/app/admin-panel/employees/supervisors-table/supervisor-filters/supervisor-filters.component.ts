import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AddSupervisorComponent } from '../../add-supervisor/add-supervisor.component';

@Component({
  selector: 'app-supervisor-filters',
  templateUrl: './supervisor-filters.component.html',
  styleUrls: ['./supervisor-filters.component.css']
})
export class SupervisorFiltersComponent implements OnInit {
  form: FormGroup;
  @Output() searchChanged = new EventEmitter<string>();

  constructor(public dialog: MatDialog) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      search: new FormControl('')
    });
  }

  onSearch() {
    this.searchChanged.emit(this.form.value.search);
  }

  onAddSupervisor() {
    this.dialog.open(AddSupervisorComponent, {
      width: '800px'
    });
  }
}
