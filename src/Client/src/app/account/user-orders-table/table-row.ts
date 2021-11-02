export class TableRow {
  constructor(
    public id: string,
    public name: string,
    public price: string,
    public size: string,
    public fee: string,
    public lowestAsk: string,
    public highestBid: string,
    public expires: string
  ) {}
}
