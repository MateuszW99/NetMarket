using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class GetAskByIdQueryHandler : IRequestHandler<GetAskByIdQuery, AskObject>
    {
        private readonly IAskService _askService;

        public GetAskByIdQueryHandler(IAskService askService)
        {
            _askService = askService;
        }

        public Task<AskObject> Handle(GetAskByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}