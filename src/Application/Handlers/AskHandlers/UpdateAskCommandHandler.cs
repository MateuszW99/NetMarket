using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.AskHandlers
{
    public class UpdateAskCommandHandler : IRequestHandler<UpdateAskCommand>
    {
        private readonly IAskService _askService;
        private IHttpService _httpService;
        private readonly ILogger<UpdateAskCommandHandler> _logger;
        
        public UpdateAskCommandHandler(IAskService askService, IHttpService httpService, ILogger<UpdateAskCommandHandler> logger)
        {
            _askService = askService;
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAskCommand request, CancellationToken cancellationToken)
        {
            var userId = Guid.Parse(_httpService.GetUserId());
            var ask = await _askService.GetAskByIdAsync(Guid.Parse(request.Id));

            if (ask == null)
            {
                throw new NotFoundException(nameof(ask), request.Id);
            }
            
            if (ask.CreatedBy != userId)
            {
                throw new ForbiddenAccessException();
            }

            await _askService.UpdateAskAsync(ask, request, userId, cancellationToken);
            _logger.LogInformation($"Updated ask {ask.Id} by {ask.CreatedBy}");
            return Unit.Value;
        }
    }
}