using Application.Common.Mappings;

namespace Application.Models.Sizes
{
    public class SizeObject : IMapFrom<SizeObject>
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}