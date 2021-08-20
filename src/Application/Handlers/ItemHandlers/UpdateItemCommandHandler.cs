using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ItemHandlers
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IItemService _itemService;
        private readonly ILogger<CreateItemCommandHandler> _logger;

        public UpdateItemCommandHandler(IItemService itemService, ILogger<CreateItemCommandHandler> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }
        
        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            await _itemService.UpdateItemAsync(request, cancellationToken);
            _logger.LogInformation($"Item updated: {request.Name}");
            return Unit.Value;
        }
    }
}