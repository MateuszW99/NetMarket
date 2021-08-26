using System;
using System.Globalization;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Models.DTOs
{
    public class AskObject : IMapFrom<Ask>
    {
        public string Id { get; set; }
        public ItemObject Item { get; set; }
        public SizeObject Size { get; set; }
        public string Price { get; set; }
        public string UserId { get; set; }
        public DateTime Expires { get; set; }
        
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Ask, AskObject>()
                .ForMember(d => d.Price, opt => opt.MapFrom(a => a.Price.ToString(CultureInfo.InvariantCulture)))
                .ForMember(d => d.Item, opt => opt.MapFrom(a => a.Item))
                .ForMember(d => d.Size, opt => opt.MapFrom(a => a.Size))
                .ForMember(d => d.UserId, opt => opt.MapFrom(a => a.CreatedBy))
                .ForMember(d => d.Expires,
                    opt => 
                        opt.MapFrom(b => b.LastModified.HasValue ? 
                            b.LastModified.Value.AddDays(30).Date :
                            b.Created.AddDays(30)));;
        }
    }
}