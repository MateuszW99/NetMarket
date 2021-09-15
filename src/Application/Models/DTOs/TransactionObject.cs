using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Models.DTOs
{
    public class TransactionObject : IMapFrom<Transaction>
    {
        public string Id { get; set; }
        public string AssignedSupervisorId { get; set; }
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