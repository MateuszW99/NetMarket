using System.Collections.Generic;
using Application.Models.DTOs;

namespace Application.Models.ApiModels.Items
{
    public class ItemCard
    {
        public ItemObject Item { get; set; }
        public AskObject LowestAsk { get; set; }
        public List<AskObject> Asks { get; set; }
        public BidObject HighestBid { get; set; }
        public List<BidObject> Bids { get; set; }
    }
}