import { Ask } from '../../ask.model';
import { Bid } from '../../bid.model';
import { Item } from '../item.model';

export class ItemDetails {
  constructor(
    public item: Item,
    public asks: Ask[],
    public bids: Bid[],
    public lowestAsk: Ask,
    public highestBid: Bid
  ) {}
}
