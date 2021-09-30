using System;
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
        private readonly IFeeService _feeService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly ILogger<CreateBidCommandHandler> _logger;
        
        public CreateBidCommandHandler(IBidService bidService,  IFeeService feeService, 
            IUserSettingsService userSettingsService, IHttpService httpService, ILogger<CreateBidCommandHandler> logger)
        {
            _bidService = bidService;
            _logger = logger;
            _feeService = feeService;
            _userSettingsService = userSettingsService;
            _httpService = httpService;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpService.GetUserId());
            var userSellerLevel = await _userSettingsService.GetUserSellerLevel(userId);
            var fee = _feeService.CalculateFee(userSellerLevel, Convert.ToDecimal(request.Price));
            
            await _bidService.CreateBidAsync(request, fee, cancellationToken);
            _logger.LogInformation("Bid created");
            return Unit.Value;
        }
    }
}