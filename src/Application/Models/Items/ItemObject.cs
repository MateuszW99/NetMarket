using Application.Common.Mappings;
using Application.Models.Brands;
using Domain.Entities;

namespace Application.Models.Items
{
    public class ItemObject : IMapFrom<Item>
    {
        public string Id { get; init; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public BrandObject Brand { get; set; }
    }
}