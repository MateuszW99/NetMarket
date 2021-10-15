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
    public class GetItemsWithCategoryQueryHandler : IRequestHandler<GetItemsWithCategoryQuery, PaginatedList<ItemObject>>
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public GetItemsWithCategoryQueryHandler(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ItemObject>> Handle(GetItemsWithCategoryQuery request, CancellationToken cancellationToken)
        {
            var queriedItems = _itemService.GetItemsWithCategory(request.Category);
            var mappedItems = await queriedItems.ProjectToListAsync<ItemObject>(_mapper.ConfigurationProvider);
            return PaginatedList<ItemObject>.Create(mappedItems, request.PageIndex, request.PageSize);
        }
    }
}