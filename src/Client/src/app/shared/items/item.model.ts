import { Brand } from './brand.model';

export class Item {
  constructor(
    public id: string,
    public name: string,
    public category: string,
    public make: string,
    public model: string,
    public gender: string,
    public retailPrice: number,
    public description: string,
    public imageUrl: string,
    public smallImageUrl: string,
    public thumbUrl: string,
    public brand: Brand,
    public lowestAsk: number,
    public highestBid: number
  ) {}
}
