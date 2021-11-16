import { Ask } from "../shared/ask.model";
import { Bid } from "../shared/bid.model";

export class Transaction {
    constructor(
        public id: string,
        public assignedSupervisorId: string,
        public companyProfit: string,
        public ask: Ask,
        public bid: Bid,
        public buyerFee: number,
        public totalBuyerCost: number,
        public status: string,
        public startDate: Date,
        public endDate: Date
    ) {}
}