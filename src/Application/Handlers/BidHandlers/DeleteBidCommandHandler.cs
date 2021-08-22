using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand>
    {
        private readonly IBidService _bidService;

        public DeleteBidCommandHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task<Unit> Handle(DeleteBidCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}