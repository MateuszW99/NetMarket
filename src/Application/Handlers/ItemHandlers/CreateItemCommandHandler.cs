using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ItemHandlers
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand>
    {
        private readonly IItemService _itemService;
        private readonly ILogger<CreateItemCommandHandler> _logger;
        
        public CreateItemCommandHandler(IItemService itemService, ILogger<CreateItemCommandHandler> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        { 
            await _itemService.CreateItemAsync(request, cancellationToken);
            _logger.LogInformation($"Item created: {request.Name}");
            return Unit.Value;
        }
    }
}