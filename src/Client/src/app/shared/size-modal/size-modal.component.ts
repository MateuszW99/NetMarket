import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SneakersSizes } from './sneakers-sizes';
import { StreetwearSizes } from './streetwear-sizes';

@Component({
  selector: 'app-size-modal',
  templateUrl: './size-modal.component.html',
  styleUrls: ['./size-modal.component.css']
})
export class SizeModalComponent implements OnInit {
  @Input() category = '';
  @Output() sizeChanged = new EventEmitter<string>();
  selectedSize = '14';
  sneakersSizes = SneakersSizes;
  streetwearSizes = StreetwearSizes;
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
