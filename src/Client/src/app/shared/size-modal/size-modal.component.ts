import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-size-modal',
  templateUrl: './size-modal.component.html',
  styleUrls: ['./size-modal.component.css']
})
export class SizeModalComponent implements OnInit {
  @Input() category = '';
  @Output() sizeChanged = new EventEmitter<string>();
  selectedSize = '14';
  clickedButton = null;
  sneakersSizes = [
    '4',
    '4.5',
    '5',
    '5.5',
    '6',
    '6.5',
    '7',
    '7.5',
    '8',
    '8.5',
    '9',
    '9.5',
    '10',
    '10.5',
    '11',
    '11.5',
    '12',
    '12.5',
    '13',
    '13.5',
    '14',
    '15',
    '16',
    '17'
  ];
  streetwearSizes = ['noSize', 'oneSize', 'S', 'M', 'L', 'XL'];
  sizes = this.sneakersSizes;

  ngOnInit(): void {
    if (this.category === 'streetwear') {
      this.sizes = this.streetwearSizes;
      this.selectedSize = 'M';
    }
  }

  onSelect(size: string): void {
    this.selectedSize = size;
  }

  onSaveChanges(): void {
    this.sizeChanged.emit(this.selectedSize);
  }
}
