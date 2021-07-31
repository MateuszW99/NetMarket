using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ItemHandlers
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
    {
        private readonly IItemService _itemService;
        private readonly ILogger<CreateItemCommandHandler> _logger;

        public DeleteItemCommandHandler(IItemService itemService, ILogger<CreateItemCommandHandler> logger)
        {
            _itemService = itemService;
            _logger = logger;
        }


        public async Task<Unit> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}