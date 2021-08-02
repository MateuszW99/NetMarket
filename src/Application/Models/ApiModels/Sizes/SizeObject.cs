using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Models
{
    public class SizeObject : IMapFrom<Size>
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}