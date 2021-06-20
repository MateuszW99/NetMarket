using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Bid : BaseEntity, IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}