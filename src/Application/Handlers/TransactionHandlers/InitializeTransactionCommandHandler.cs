using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Transactions.Commands;
using Domain;
using MediatR;
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

        public InitializeTransactionCommandHandler(
            ITransactionService transactionService,
            ILogger<InitializeTransactionCommandHandler> logger, 
            IAskService askService,
            IBidService bidService, ISupervisorService supervisorService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _askService = askService;
            _bidService = bidService;
            _supervisorService = supervisorService;
        }

        public async Task<Unit> Handle(InitializeTransactionCommand request, CancellationToken cancellationToken)
        {
            var ask = await _askService.GetAskByIdAsync(Guid.Parse(request.AskId));
            var bid = await _bidService.GetBidByIdAsync(Guid.Parse(request.BidId));
            var supervisorId = await _supervisorService.GetLeastLoadedSupervisorId(Roles.Supervisor);
            await _transactionService.InitializeTransaction(ask, bid, DateTime.Now, supervisorId, cancellationToken);
            _logger.LogInformation($"New transaction created for Ask {ask.Id} and Bid {bid.Id}");
            return Unit.Value;
        }
    }
}