using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Supervisor : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; set; }
        
        
        
        public List<DomainEvent> DomainEvents { get; set; }
    }
}