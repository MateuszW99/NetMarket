using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.AskHandlers
{
    public class CreateAskCommandHandler : IRequestHandler<CreateAskCommand>
    {
        private readonly IAskService _askService;
        private readonly ILogger<CreateAskCommandHandler> _logger;
        
        public CreateAskCommandHandler(IAskService askService, ILogger<CreateAskCommandHandler> logger)
        {
            _askService = askService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateAskCommand request, CancellationToken cancellationToken)
        {
            await _askService.CreateAskAsync(request, cancellationToken);
            _logger.LogInformation("Ask created");
            return Unit.Value;
        }
    }
}