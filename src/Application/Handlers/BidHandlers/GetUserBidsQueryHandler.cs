using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class GetUserBidsQueryHandler : IRequestHandler<GetUserBidsQuery, PaginatedList<BidObject>>
    {
        private readonly IBidService _bidService;

        public GetUserBidsQueryHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task<PaginatedList<BidObject>> Handle(GetUserBidsQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}