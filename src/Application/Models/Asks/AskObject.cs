using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Models
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
                .ForMember(d => d.Item, opt => opt.MapFrom(a => a.ItemSize.Item))
                .ForMember(d => d.Size, opt => opt.MapFrom(a => a.ItemSize.Size))
                .ForMember(d => d.UserId, opt => opt.MapFrom(a => a.CreatedBy));
        }
    }
}