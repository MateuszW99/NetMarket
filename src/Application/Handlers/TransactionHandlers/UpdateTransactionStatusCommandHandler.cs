using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Transactions.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.TransactionHandlers
{
    public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand>
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<UpdateTransactionStatusCommandHandler> _logger;
        private readonly IHttpService _httpService;

        public UpdateTransactionStatusCommandHandler(ITransactionService transactionService,
            ILogger<UpdateTransactionStatusCommandHandler> logger, IHttpService httpService)
        {
            _transactionService = transactionService;
            _logger = logger;
            _httpService = httpService;
        }

        public async Task<Unit> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
        {
            var supervisorId = _httpService.GetUserId();
            await _transactionService.UpdateTransactionStatusAsync(request, supervisorId, cancellationToken);
            _logger.LogInformation($"Transaction status updated: {request.Id}");
            return Unit.Value;
        }
    }
}