using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.ItemHandlers
{
    public class UpdateItemCommandHandler : IRequest<UpdateItemCommand>
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
            return Unit.Value;
        }
    }
}