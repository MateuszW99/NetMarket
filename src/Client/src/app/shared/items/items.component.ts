import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent {
  @Input() category = '';
  @Input() bannerDescription = '';
  @Input() bannerImg = '';
  @Input() brands: string[] = [];
  @Input() genders: string[] = [];
}
