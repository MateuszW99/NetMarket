import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-fee-info',
  templateUrl: './fee-info.component.html',
  styleUrls: ['./fee-info.component.css']
})
export class FeeInfoComponent {
  @Input() category: string;
  @Input() sellerLevel: string;
  fees: Map<string, string> = new Map([
    ['Beginner', '10%'],
    ['Intermediate', '8.5%'],
    ['Advanced', '6%'],
    ['Business', '4%']
  ]);

  getFee(): string {
    return this.fees.get(this.sellerLevel);
  }
}
