using System.Threading;
using System.Threading.Tasks;
using Application.Common.Events;
using Application.Common.Interfaces;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.TransactionHandlers
{
    public class TransactionInitializedEventHandler : INotificationHandler<DomainEventNotification<TransactionInitializedEvent>>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILogger<TransactionInitializedEventHandler> _logger;

        public TransactionInitializedEventHandler(IUserSettingsService userSettingsService, ILogger<TransactionInitializedEventHandler> logger)
        {
            _userSettingsService = userSettingsService;
            _logger = logger;
        }

        public async Task Handle(DomainEventNotification<TransactionInitializedEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;
            if (await _userSettingsService.TryUpdateUserSellerLevel(domainEvent.SellerId, cancellationToken))
            {
                _logger.LogInformation($"Updated SellerLevel of user: {domainEvent.SellerId}");
            }
        }
    }
}