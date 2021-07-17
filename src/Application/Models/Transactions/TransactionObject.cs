using System;
using Application.Models.Asks;
using Application.Models.Bids;

namespace Application.Models.Transactions
{
    public class TransactionObject
    {
        public string Id { get; set; }
        public AskObject Ask { get; set; }
        public BidObject Bid { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal SellerFee { get; set; }
        public decimal BuyerFee { get; set; }
        public decimal Payout { get; set; }
    }
}