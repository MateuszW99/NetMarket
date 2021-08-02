using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.DTOs
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
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Item, ItemObject>()
                .ForMember(d => d.Brand, opt => opt.MapFrom(a => a.Brand));
        }
    }
}