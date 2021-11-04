import { Component, DoCheck, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/auth/auth.service';
import { ItemsService } from "../shared/items/items.service";
import { ItemsParams } from "../shared/items/items-params";
import { RoutingService } from "../shared/services/routing/routing.service";

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit, DoCheck {
  form: FormGroup;
  logoPath: string;
  searchIconPath: string;
  dropdownIconPath: string;
  accountIconPath: string;
  helpIconPath: string;
  isUserLoggedIn: boolean;

  categories: string[] = [ 'sneakers', 'electronics', 'streetwear', 'collectibles' ];
  currentCategory: string;

  constructor(private authService: AuthService,
              private itemsService: ItemsService,
              private routingService: RoutingService
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      searchText: new FormControl('', Validators.maxLength(100)),
      category: new FormControl(this.categories[0], Validators.maxLength(20))
    });

    this.logoPath = '../../../assets/logo.svg';
    this.searchIconPath = '../../../assets/search.svg';
    this.dropdownIconPath = '../../../assets/expand_more.svg';
    this.accountIconPath = '../../../assets/account.svg';
    this.helpIconPath  = '../../../assets/help.svg';

    this.isUserLoggedIn = this.authService.isUserLoggedIn();
  }

  resolveCategory(): string {
    if (this.currentCategory === '/' || this.currentCategory === '') {
      this.currentCategory = this.categories[0];
    }
    return this.currentCategory;
  }

  onSearch(): void {
    this.itemsService.getItems(new ItemsParams(15, 1, this.form.value.category, this.form.value.searchText));
}

  logout(): void {
    this.authService.logout();
  }

  ngDoCheck(): void {
    this.currentCategory = this.routingService.getCurrentRoute();
  }
}
