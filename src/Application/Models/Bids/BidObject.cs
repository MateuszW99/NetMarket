using Application.Common.Mappings;
using Application.Models.Items;
using Application.Models.Sizes;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.Bids
{
    public class BidObject : IMapFrom<Bid>
    {
        public string Id { get; set; }
        public ItemObject Item { get; set; }
        public SizeObject Size { get; set; }
        public decimal Price { get; set; }
        public bool IsCanceled { get; set; }
        public string UserId { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Bid, BidObject>()
                .ForMember(d => d.Size, opt => opt.MapFrom(b => b.ItemSize))
                .ForMember(d => d.UserId, opt => opt.MapFrom(b => b.CreatedBy));
        }
    }
    
   
}