using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Models
{
    public class BrandObject : IMapFrom<Brand>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}