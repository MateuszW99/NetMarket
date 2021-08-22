using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class UpdateBidCommandHandler : IRequestHandler<UpdateBidCommand>
    {
        private readonly IBidService _bidService;

        public UpdateBidCommandHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task<Unit> Handle(UpdateBidCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}