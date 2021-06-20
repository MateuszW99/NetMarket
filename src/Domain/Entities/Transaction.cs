using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Transaction : IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
}