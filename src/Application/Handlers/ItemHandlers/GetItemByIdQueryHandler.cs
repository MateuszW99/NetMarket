using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Models.ApiModels.Items;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ItemHandlers
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemCard>
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        private readonly IAskService _askService;
        private readonly IBidService _bidService;
        
        public GetItemByIdQueryHandler(IItemService itemService, IMapper mapper, IAskService askService, IBidService bidService)
        {
            _itemService = itemService;
            _mapper = mapper;
            _askService = askService;
            _bidService = bidService;
        }

        public async Task<ItemCard> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var itemId = Guid.Parse(request.Id);
            var item = await _itemService.GetItemByIdAsync(itemId);
            
            var asks = _askService.GetItemAsks(itemId);
            var bids = _bidService.GetItemBids(itemId);

            var lowestAsk = await asks.OrderBy(x => x.Price).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            var highestBid = await bids.OrderByDescending(x => x.Price).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var mappedItem = _mapper.Map<ItemObject>(item);
            mappedItem.LowestAsk = lowestAsk.Price.ToString(CultureInfo.InvariantCulture);
            
            return new ItemCard()
            {
                Item = mappedItem,
                Asks = await asks.ProjectToListAsync<AskObject>(_mapper.ConfigurationProvider),
                Bids = await bids.ProjectToListAsync<BidObject>(_mapper.ConfigurationProvider),
                LowestAsk = _mapper.Map<AskObject>(lowestAsk),
                HighestBid = _mapper.Map<BidObject>(highestBid)
            };
        }
    }
}