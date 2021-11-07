using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Transactions.Commands;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.TransactionHandlers
{
    public class InitializeTransactionCommandHandler : IRequestHandler<InitializeTransactionCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<InitializeTransactionCommandHandler> _logger;
        private readonly IAskService _askService;
        private readonly IBidService _bidService;
        private readonly ISupervisorService _supervisorService;
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly IFeeService _feeService;
        
        public InitializeTransactionCommandHandler(
            ITransactionService transactionService,
            ILogger<InitializeTransactionCommandHandler> logger, 
            IAskService askService, 
            IBidService bidService, 
            ISupervisorService supervisorService,
            IUserSettingsService userSettingsService, 
            IHttpService httpService,
            IFeeService feeService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _askService = askService;
            _bidService = bidService;
            _supervisorService = supervisorService;
            _userSettingsService = userSettingsService;
            _httpService = httpService;
            _feeService = feeService;
        }

        public async Task<Unit> Handle(InitializeTransactionCommand request, CancellationToken cancellationToken)
        {
            Ask ask;
            Bid bid;

            var userId = Guid.Parse(_httpService.GetUserId());
            var userSellerLevel = await _userSettingsService.GetUserSellerLevel(userId);
            
            if (!string.IsNullOrEmpty(request.AskId)) // Buy now
            {
                ask = await _askService.GetAskByIdAsync(Guid.Parse(request.AskId));
                var fee = await _feeService.CalculateFee(userSellerLevel, ask.Price);
                
                bid = new()
                {
                    ItemId = ask.ItemId,
                    SizeId = ask.SizeId,
                    Price = ask.Price,
                    BuyerFee = fee
                };
                await _bidService.CreateBidAsync(bid, cancellationToken);
            }
            else if (!string.IsNullOrEmpty(request.BidId)) // Sell now
            {
                bid = await _bidService.GetBidByIdAsync(Guid.Parse(request.BidId));
                var fee = await _feeService.CalculateFee(userSellerLevel, bid.Price);

                ask = new()
                {
                    ItemId = bid.ItemId,
                    SizeId = bid.SizeId,
                    Price = bid.Price,
                    SellerFee = fee
                };
                await _askService.CreateAskAsync(ask, cancellationToken);
            }
            else
            {
                throw new BadHttpRequestException("Wrong AskId or BidId");
            }

            if (ask.CreatedBy == bid.CreatedBy)
            {
                throw new FraudDetectedException($"Fraud detected for user: {ask.CreatedBy}");
            }
            
            var supervisorId = await _supervisorService.GetLeastLoadedSupervisorId(Roles.Supervisor);
            await _transactionService.InitializeTransaction(ask, bid, DateTime.Now, supervisorId, cancellationToken);
            _logger.LogInformation($"New transaction created for Ask {ask.Id} and Bid {bid.Id}");
            return Unit.Value;
        }
    }
}