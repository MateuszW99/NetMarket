using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using Domain.Entities;
using MediatR;

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

            if (item == null)
            {
                throw new NotFoundException(nameof(Item), itemId);
            }
            
            var itemObject = _mapper.Map<ItemObject>(item);

            var asks = await _askService.GetItemAsks(itemId);
            var lowestAsk = asks.FirstOrDefault();
            var bids = await _bidService.GetItemBids(itemId);
            var highestBid = bids.LastOrDefault();

            return new ItemCard()
            {
                Item = itemObject,
                Asks = _mapper.Map<List<AskObject>>(asks),
                Bids = _mapper.Map<List<BidObject>>(bids),
                LowestAsk = lowestAsk is null ? new AskObject() : _mapper.Map<AskObject>(lowestAsk),
                HighestBid = highestBid is null ? new BidObject() : _mapper.Map<BidObject>(highestBid)
            };
        }
    }
}