﻿using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Bid : BaseEntity, IHasDomainEvent
    {        
        public Guid Id { get; init; }
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
        public decimal Price { get; set; }
        public decimal BuyerFee { get; set; }
        public bool UsedInTransaction { get; set; }
        public List<DomainEvent> DomainEvents { get; set; }
    }
}