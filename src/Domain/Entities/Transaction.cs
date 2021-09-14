using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Transaction : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; init; }
        public Guid AssignedSupervisorId { get; set; }
        
        public Guid AskId { get; set; }
        public Ask Ask { get; set; }
        
        public Guid BidId { get; set; }
        public Bid Bid { get; set; }
        
        public TransactionStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SellerFee { get; set; }
        public decimal BuyerFee { get; set; }
        public decimal Payout { get; set; }

        public List<DomainEvent> DomainEvents { get; set; }
    }
}