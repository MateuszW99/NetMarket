using System;
using System.Collections.Generic;

namespace Domain.Common
{
    public interface IHasDomainEvent
    {
        public List<DomainEvent> DomainEvents { get; set; }
    }
    
    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            OccurenceDate = DateTimeOffset.UtcNow;
        }
        
        public bool IsPublished { get; set; }
        public DateTimeOffset OccurenceDate { get; protected set; }
    }
}