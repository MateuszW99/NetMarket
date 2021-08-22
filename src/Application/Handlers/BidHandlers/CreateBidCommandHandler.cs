using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IBidService _bidService;

        public CreateBidCommandHandler(IBidService bidService)
        {
            _bidService = bidService;
        }

        public Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}