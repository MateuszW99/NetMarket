export class UserTransactionRow {
  constructor(
    public name: string,
    public category: string,
    public startDate: string,
    public totalBuyerCost: string,
    public status: string
  ) {}
}
