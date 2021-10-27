import { Component, Input, OnInit } from '@angular/core';
import { Ask } from 'src/app/shared/ask.model';
import { Bid } from 'src/app/shared/bid.model';
import { TableRow } from './table-row';

@Component({
  selector: 'app-order-table',
  templateUrl: './order-table.component.html',
  styleUrls: ['./order-table.component.css']
})
export class OrderTableComponent implements OnInit {
  @Input() data: Ask[] | Bid[];
  activity: string;
  category: string;
  empty = true;
  displayedColumns = [
    'name',
    'price',
    'size',
    'fee',
    'lowestAsk',
    'highestBid',
    'expires'
  ];
  dataSource: TableRow[] = [];

  ngOnInit(): void {
    this.empty = this.data.length === 0;
    this.data.forEach((element: Ask | Bid) => {
      console.log(element);

      const row = new TableRow(
        element.item.name,
        element.price.toString(),
        element.size.value,
        element instanceof Ask
          ? element.sellerFee.toString()
          : element.buyerFee.toString(),
        element.item.lowestAsk === null
          ? 'No asks'
          : element.item.lowestAsk.toString(),
        'todo: highest bid',
        //element.item.highestBid === null ? 'No bids' : element.item.highestBid.toString(),
        element.expires.toString()
      );

      this.dataSource.push(row);
    });
  }
}
