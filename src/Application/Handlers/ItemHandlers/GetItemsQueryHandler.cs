using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.ItemHandlers
{
    public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, PaginatedList<ItemObject>>
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public GetItemsQueryHandler(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ItemObject>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            var queriedItems = await _itemService.GetItemsWithQuery(request.SearchQuery);
            var mappedItems = _mapper.Map<List<ItemObject>>(queriedItems);
            return PaginatedList<ItemObject>.Create(mappedItems, request.PageIndex, request.PageSize);
        }
    }
}