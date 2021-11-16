using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using MediatR;

namespace Application.Handlers.ItemHandlers
{
    public class DeleteItemCommandHandler: IRequestHandler<DeleteItemCommand>
    {
        private readonly IItemService _itemService;
        
        public DeleteItemCommandHandler(IItemService itemService)
        {
            _itemService = itemService;
        }

        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
          
            var item = await _itemService.GetItemByIdAsync(Guid.Parse(request.Id));

            if (item == null)
            {
                throw new NotFoundException(nameof(item), request.Id);
            }
            
            await _itemService.DeleteItemAsync(item, cancellationToken);
            return Unit.Value;
        }
    }
}