using System;
using Domain.Common;

namespace Domain.Events
{
    public class TransactionInitializedEvent : DomainEvent
    {
        public TransactionInitializedEvent(Guid sellerId)
        {
            SellerId = sellerId;
        }
        
        public Guid SellerId { get; }
    }
}