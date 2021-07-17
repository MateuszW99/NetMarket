using Application.Models.Items;
using Application.Models.Sizes;

namespace Application.Models.Asks
{
    public class AskObject
    {
        public string Id { get; set; }
        public ItemObject Item { get; set; }
        public SizeObject Size { get; set; }
        public decimal Price { get; set; }
        public bool IsCanceled { get; set; }
        public string UserId { get; set; }
    }
}