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
    public class DeleteAskCommandHandler : IRequestHandler<DeleteAskCommand>
    {
        private readonly IAskService _askService;
        private readonly IHttpService _httpService;
        private readonly ILogger<DeleteAskCommandHandler> _logger;
        
        public DeleteAskCommandHandler(IAskService askService, IHttpService httpService, ILogger<DeleteAskCommandHandler> logger)
        {
            _askService = askService;
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteAskCommand request, CancellationToken cancellationToken)
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

            await _askService.DeleteAskAsync(ask, cancellationToken);
            _logger.LogInformation($"Deleted ask {ask.Id}");
            return Unit.Value;
        }
    }
}