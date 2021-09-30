using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.AskHandlers
{
    public class CreateAskCommandHandler : IRequestHandler<CreateAskCommand>
    {
        private readonly IAskService _askService;
        private readonly IFeeService _feeService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly ILogger<CreateAskCommandHandler> _logger;

        public CreateAskCommandHandler(IAskService askService, IFeeService feeService,
            ILogger<CreateAskCommandHandler> logger, IUserSettingsService userSettingsService, IHttpService httpService)
        {
            _askService = askService;
            _logger = logger;
            _feeService = feeService;
            _userSettingsService = userSettingsService;
            _httpService = httpService;
        }

        public async Task<Unit> Handle(CreateAskCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpService.GetUserId());
            var userSellerLevel = await _userSettingsService.GetUserSellerLevel(userId);
            var fee = _feeService.CalculateFee(userSellerLevel, Convert.ToDecimal(request.Price));
            
            await _askService.CreateAskAsync(request, fee, cancellationToken);
            _logger.LogInformation("Ask created");
            return Unit.Value;
        }
    }
}