using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.BidHandlers
{
    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IBidService _bidService;
        private readonly ILogger<CreateBidCommandHandler> _logger;
        
        public CreateBidCommandHandler(IBidService bidService, ILogger<CreateBidCommandHandler> logger)
        {
            _bidService = bidService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            await _bidService.CreateBidAsync(request, cancellationToken);
            _logger.LogInformation("Bid created");
            return Unit.Value;
        }
    }
}