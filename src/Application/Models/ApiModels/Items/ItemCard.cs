using System.Collections.Generic;
using Application.Models.DTOs;

namespace Application.Models.ApiModels.Items
{
    public class ItemCard
    {
        public ItemObject Item { get; set; }
        public List<AskObject> Asks { get; set; }
        public List<BidObject> Bids { get; set; }
    }
}