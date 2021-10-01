import { Component } from '@angular/core';

@Component({
  selector: 'app-sneakers',
  templateUrl: './sneakers.component.html',
  styleUrls: ['./sneakers.component.css']
})
export class SneakersComponent {
  category = 'sneakers';
  bannerDescription =
    'Buy and sell new sneakers & shoes from Air Jordan, adidas, Nike and more!';
  bannerImg = '/assets/banners/sneakers.svg';

  //These brands will be displayed in the filters
  brands: string[] = ['All', 'Adidas', 'Jordan', 'Nike', 'Yeezy', 'Other'];

  //These genders will be displayed in the filters
  genders: string[] = [
    'All',
    'Men',
    'Women',
    'Child',
    'Preschool',
    'Infant',
    'Toddler'
  ];
}
