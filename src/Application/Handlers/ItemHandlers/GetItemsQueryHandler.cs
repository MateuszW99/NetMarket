using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
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
            var items = await _itemService.GetItemsAsync(request.SearchQuery, request.PageSize, request.PageNumber);
            var itemObjects = await items.AsQueryable()
                .ProjectToListAsync<ItemObject>(_mapper.ConfigurationProvider);
            
            return PaginatedList<ItemObject>.Create(itemObjects, request.PageNumber, request.PageSize);
        }
    }
}