using System;

namespace Domain.Entities
{
    public class ItemSize
    {
        public Guid Id { get; set; }
        
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        
        public Guid SizeId { get; set; }
        public Size Size { get; set; }
    }
}