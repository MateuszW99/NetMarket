using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Bid : BaseEntity, IHasDomainEvent
    {        
        public Guid Id { get; set; }
        public Guid ItemSizeId { get; set; }
        public ItemSize ItemSize { get; set; }
        public decimal Price { get; set; }
        public bool IsCanceled { get; set; }
        
        public List<DomainEvent> DomainEvents { get; set; }
    }
}