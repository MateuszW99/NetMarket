using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class UpdateAskCommandHandler : IRequestHandler<UpdateAskCommand>
    {
        private readonly IAskService _askService;

        public UpdateAskCommandHandler(IAskService askService)
        {
            _askService = askService;
        }

        public Task<Unit> Handle(UpdateAskCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}