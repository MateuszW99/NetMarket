import { Component } from '@angular/core';

@Component({
  selector: 'app-streetwear',
  templateUrl: './streetwear.component.html',
  styleUrls: ['./streetwear.component.css']
})
export class StreetwearComponent {
  category = 'streetwear';
  bannerDescription =
    'Buy and sell new streetwear from Supreme, Bape, Fear of God, OFF-WHITE and more!';
  bannerImg = '/assets/banners/streetwear.svg';

  //These brands will be displayed in the filters
  brands: string[] = [
    'All',
    'Bape',
    'Kith',
    'Palace',
    'Fear Of God',
    'Of White'
  ];

  //These genders will be displayed in the filters
  genders: string[] = ['All', 'Men', 'Women', 'Kids', 'Unisex'];
}
