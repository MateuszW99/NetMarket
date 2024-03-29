import { Item } from './items/item.model';
import { Size } from './size.model';

export class Ask {
  constructor(
    public id: string,
    public item: Item,
    public size: Size,
    public price: number,
    public userId: string,
    public expires: Date,
    public sellerFee: number
  ) {}
}
