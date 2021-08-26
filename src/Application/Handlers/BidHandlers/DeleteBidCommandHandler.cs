using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.BidHandlers
{
    public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand>
    {
        private readonly IBidService _bidService;
        private readonly IHttpService _httpService;
        private readonly ILogger<DeleteBidCommandHandler> _logger;

        public DeleteBidCommandHandler(IBidService bidService, IHttpService httpService,ILogger<DeleteBidCommandHandler> logger)
        {
            _bidService = bidService;
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBidCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpService.GetUserId());
            var bid = await _bidService.GetBidByIdAsync(Guid.Parse(request.Id));
            
            if (bid == null)
            {
                throw new NotFoundException(nameof(bid), request.Id);
            }
            
            if (bid.CreatedBy != userId)
            {
                throw new UnauthorizedAccessException($"Authorization rules violated by user {userId}");
            }
            
            await _bidService.DeleteBidAsync(bid, cancellationToken);
            _logger.LogInformation($"Deleted bid {request.Id}");
            return Unit.Value;
        }
    }
}