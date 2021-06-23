﻿using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Item : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        
        public Guid BrandId { get; set; }
        public Brand Brand { get; set; }
        
        public List<DomainEvent> DomainEvents { get; set; }
    }
}