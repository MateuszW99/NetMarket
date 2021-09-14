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
    public class UpdateBidCommandHandler : IRequestHandler<UpdateBidCommand>
    {
        private readonly IBidService _bidService;
        private IHttpService _httpService;
        private readonly ILogger<UpdateBidCommandHandler> _logger;

        public UpdateBidCommandHandler(IBidService bidService, IHttpService httpService, ILogger<UpdateBidCommandHandler> logger)
        {
            _bidService = bidService;
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateBidCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpService.GetUserId());
            var bid = await _bidService.GetBidByIdAsync(Guid.Parse(request.Id));

            if (bid == null)
            {
                throw new NotFoundException(nameof(bid), request.Id);
            }
            
            if (bid.CreatedBy != userId)
            {
                throw new ForbiddenAccessException();
            }

            await _bidService.UpdateBidAsync(bid, request, userId, cancellationToken);
            _logger.LogInformation($"Updated bid {bid.Id} by {bid.CreatedBy}");
            return Unit.Value;
        }
    }
}