using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class GetUserAsksQueryHandler : IRequestHandler<GetUserAsksQuery, PaginatedList<AskObject>>
    {
        private readonly IAskService _askService;

        public GetUserAsksQueryHandler(IAskService askService)
        {
            _askService = askService;
        }

        public Task<PaginatedList<AskObject>> Handle(GetUserAsksQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}