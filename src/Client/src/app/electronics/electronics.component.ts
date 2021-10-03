import { Component } from '@angular/core';

@Component({
  selector: 'app-electronics',
  templateUrl: './electronics.component.html',
  styleUrls: ['./electronics.component.css']
})
export class ElectronicsComponent {
  category = 'electronics';
  bannerDescription = 'Buy and sell new consoles, graphic cards and more!';
  bannerImg = '/assets/banners/electronics.svg';
  brands: string[] = [
    'All',
    'Apple',
    'AMD',
    'Intel',
    'Microsoft',
    'NVIDIA',
    'Playstation',
    'Xbox',
    'Samsung',
    'Razer',
    'Other'
  ];
}
