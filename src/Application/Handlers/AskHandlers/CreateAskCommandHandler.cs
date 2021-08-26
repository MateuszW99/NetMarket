using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class CreateAskCommandHandler : IRequestHandler<CreateAskCommand>
    {
        private readonly IAskService _askService;

        public CreateAskCommandHandler(IAskService askService)
        {
            _askService = askService;
        }

        public async Task<Unit> Handle(CreateAskCommand request, CancellationToken cancellationToken)
        {
            await _askService.CreateAskAsync(request, cancellationToken);
            return Unit.Value;
        }
    }
}