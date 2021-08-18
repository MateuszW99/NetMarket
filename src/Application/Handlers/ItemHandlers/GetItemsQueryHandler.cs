using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            var queredItems = _itemService.GetItemsWithQuery(request.SearchQuery);
            var mappedItems = await queredItems.ProjectToListAsync<ItemObject>(_mapper.ConfigurationProvider);
            return PaginatedList<ItemObject>.Create(mappedItems, request.PageIndex, request.PageSize);
        }
    }
}