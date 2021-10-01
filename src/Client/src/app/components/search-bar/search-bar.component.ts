import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import { AuthService } from 'src/app/auth/auth.service';
import { ItemsService } from "../../shared/items/items.service";
import {ItemsParams} from "../../shared/items/items-params";

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  form: FormGroup;
  logoPath: string;
  searchIconPath: string;
  dropdownIconPath: string;
  accountIconPath: string;
  helpIconPath: string;
  isUserLoggedIn: boolean;

  constructor(private authService: AuthService, private itemsService: ItemsService) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      searchText: new FormControl('', Validators.maxLength(100))
    });

    this.logoPath = '../../../assets/logo.svg';
    this.searchIconPath = '../../../assets/search.svg';
    this.dropdownIconPath = '../../../assets/expand_more.svg';
    this.accountIconPath = '../../../assets/account.svg';
    this.helpIconPath  = '../../../assets/help.svg';
    this.isUserLoggedIn = this.authService.isUserLoggedIn();
  }

  onSearch(): void {

    this.itemsService.getItems(new ItemsParams(15, 1, 'sneakers', this.form.value.searchText));
  }

  logout(): void {
    this.authService.logout();
  }
}
