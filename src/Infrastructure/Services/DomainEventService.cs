using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        
        public DomainEventService(ILogger<DomainEventService> logger)
        {
            _logger = logger;
        }
        
        public Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation($"Publishing {domainEvent.GetType().Namespace} event.");
            return Task.CompletedTask; // TODO: add mediator publish method
        }
    }
}