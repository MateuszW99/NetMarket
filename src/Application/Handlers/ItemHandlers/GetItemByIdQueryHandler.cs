using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
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
            
            var asks = _askService.GetItemAsks(itemId);
            var bids = _bidService.GetItemBids(itemId);
            
            return new ItemCard()
            {
                Item = _mapper.Map<ItemObject>(item),
                Asks = await asks.ProjectToListAsync<AskObject>(_mapper.ConfigurationProvider),
                Bids = await bids.ProjectToListAsync<BidObject>(_mapper.ConfigurationProvider)
            };
        }
    }
}