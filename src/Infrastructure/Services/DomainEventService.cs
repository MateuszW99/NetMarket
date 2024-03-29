﻿using System;
using System.Threading.Tasks;
using Application.Common.Events;
using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IMediator _mediator;
        
        public DomainEventService(ILogger<DomainEventService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        
        public async Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation($"Publishing {domainEvent.GetType()} event.");
            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>)
                    .MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}