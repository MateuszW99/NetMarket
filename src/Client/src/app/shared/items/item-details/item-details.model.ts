import { Ask } from '../../ask.model';
import { Bid } from '../../bid.model';
import { Item } from '../item.model';

export class ItemDetails {
  constructor(
    public item: Item,
    public lowestAsk: Ask,
    public asks: Ask[],
    public highestBid: Bid,
    public bids: Bid[]
  ) {}
}
