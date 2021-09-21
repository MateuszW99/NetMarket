import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  searchForm: FormGroup;

  ngOnInit(): void {
    this.searchForm = new FormGroup({
      name: new FormControl('')
    });
  }

  onSearch(): void {
    console.log(this.searchForm.value.name.trim());
    //TODO search sneakers with applied filters
  }

  onRemoveFilters(): void {
    this.searchForm.reset();

    // TODO remove filters
    console.log('Filters removed');
  }
}
