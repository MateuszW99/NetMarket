import { Component } from '@angular/core';

@Component({
  selector: 'app-collectibles',
  templateUrl: './collectibles.component.html',
  styleUrls: ['./collectibles.component.css']
})
export class CollectiblesComponent {
  category = 'collectibles';
  bannerDescription = 'Buy and sell many kinds of collectibles!';
  bannerImg = '/assets/banners/collectibles.svg';
}
