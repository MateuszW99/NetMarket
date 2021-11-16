import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-transactions-filters',
  templateUrl: './transactions-filters.component.html',
  styleUrls: ['./transactions-filters.component.css']
})
export class TransactionsFiltersComponent implements OnInit {
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
}
