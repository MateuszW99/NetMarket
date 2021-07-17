using Application.Common.Mappings;
using Application.Models.Items;
using Application.Models.Sizes;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.Asks
{
    public class AskObject : IMapFrom<Ask>
    {
        public string Id { get; set; }
        public ItemObject Item { get; set; }
        public SizeObject Size { get; set; }
        public decimal Price { get; set; }
        public bool IsCanceled { get; set; }
        public string UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ask, AskObject>()
                .ForMember(d => d.Size, opt => opt.MapFrom(a => a.ItemSize))
                .ForMember(d => d.UserId, opt => opt.MapFrom(a => a.CreatedBy));
        }
    }
}