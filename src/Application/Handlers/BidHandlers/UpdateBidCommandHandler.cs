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
        private readonly IFeeService _feeService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly ILogger<UpdateBidCommandHandler> _logger;

        public UpdateBidCommandHandler(IBidService bidService, IHttpService httpService, 
            IFeeService feeService, IUserSettingsService userSettingsService,
            ILogger<UpdateBidCommandHandler> logger)
        {
            _bidService = bidService;
            _httpService = httpService;
            _logger = logger;
            _feeService = feeService;
            _userSettingsService = userSettingsService;
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

            var userSellerLevel = await _userSettingsService.GetUserSellerLevel(userId);
            var fee = _feeService.CalculateFee(userSellerLevel, Convert.ToDecimal(request.Price));
            
            await _bidService.UpdateBidAsync(bid, request, fee, cancellationToken);
            _logger.LogInformation($"Updated bid {bid.Id} by {bid.CreatedBy}");
            return Unit.Value;
        }
    }
}