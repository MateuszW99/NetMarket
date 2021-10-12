using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Item : IHasDomainEvent
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Gender { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        public ICollection<Ask> Asks { get; set; }
        public ICollection<Bid> Bids { get; set; }
        public List<DomainEvent> DomainEvents { get; set; }
    }
}