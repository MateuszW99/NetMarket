import { Item } from 'src/app/shared/items/item.model';
import { Size } from 'src/app/shared/size.model';

export class Bid {
  constructor(
    public id: string,
    public item: Item,
    public size: Size,
    public price: number,
    public userId: string,
    public expires: Date,
    public buyerFee: number
  ) {}
}
