using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidObject>
    {
        private readonly IBidService _bidService;

        public GetBidByIdQueryHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task<BidObject> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}