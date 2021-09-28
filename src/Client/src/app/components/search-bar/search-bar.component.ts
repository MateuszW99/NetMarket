import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  form: FormGroup;
  dropdownIconPath: string;
  isDropdownExpanded: boolean;
  isUserLoggedIn: boolean;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.form = new FormGroup({});

    this.isDropdownExpanded = false;
    this.dropdownIconPath = '../../../assets/expand_more.svg';
    this.isUserLoggedIn = this.authService.isUserLoggedIn();
  }

  onSearch(): void {}

  hideDropdown(): void {
    this.isDropdownExpanded = false;
    this.dropdownIconPath = '../../../assets/expand_more.svg';
  }

  showDropdown(): void {
    this.isDropdownExpanded = true;
    this.dropdownIconPath = '../../../assets/expand_less.svg';
  }

  logout(): void {
    this.authService.logout();
  }
}
