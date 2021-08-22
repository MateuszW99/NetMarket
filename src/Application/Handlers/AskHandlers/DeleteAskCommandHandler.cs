using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class DeleteAskCommandHandler : IRequestHandler<DeleteAskCommand>
    {
        private readonly IAskService _askService;

        public DeleteAskCommandHandler(IAskService askService)
        {
            _askService = askService;
        }

        public Task<Unit> Handle(DeleteAskCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}