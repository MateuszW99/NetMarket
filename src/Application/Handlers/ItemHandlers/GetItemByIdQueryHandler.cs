using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.ItemHandlers
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemObject>
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        
        public GetItemByIdQueryHandler(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        public async Task<ItemObject> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _itemService.GetItemById(Guid.Parse(request.Id));
            return _mapper.Map<ItemObject>(item);
        }
    }
}