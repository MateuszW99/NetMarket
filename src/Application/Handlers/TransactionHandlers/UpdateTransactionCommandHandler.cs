using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Transactions.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.TransactionHandlers
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<UpdateTransactionCommandHandler> _logger;

        public UpdateTransactionCommandHandler(ITransactionService transactionService,
            ILogger<UpdateTransactionCommandHandler> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            await _transactionService.UpdateTransactionAsync(request, cancellationToken);
            _logger.LogInformation($"Transaction updated: {request.Id}");
            return Unit.Value;
        }
    }
}