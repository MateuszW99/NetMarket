using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ItemHandlers
{
    public class GetTrendingItemsQueryHandler : IRequestHandler<GetTrendingItemsQuery, List<ItemCard>>
    {
        private readonly IItemService _itemService;
        private readonly IAskService _askService;
        private readonly IMapper _mapper;

        public GetTrendingItemsQueryHandler(IItemService itemService, IMapper mapper, IAskService askService)
        {
            _itemService = itemService;
            _mapper = mapper;
            _askService = askService;
        }


        public async Task<List<ItemCard>> Handle(GetTrendingItemsQuery request, CancellationToken cancellationToken)
        {
            var trendingItems = new List<ItemCard>();
            var items = _itemService.GetTrendingItems(request.Category, request.Count).ToList();
            
            foreach (var item in items)
            {
                var lowestAsk = await _askService.GetItemAsks(item.Id)
                    .OrderBy(x => x.Price)
                    .FirstOrDefaultAsync(cancellationToken);
                
                trendingItems.Add(new ItemCard()
                {
                    Item = _mapper.Map<ItemObject>(item),
                    LowestAsk = _mapper.Map<AskObject>(lowestAsk)
                });
            }

            return trendingItems;
        }
    }
}