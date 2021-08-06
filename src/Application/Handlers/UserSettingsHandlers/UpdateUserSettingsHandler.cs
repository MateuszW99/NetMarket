using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.UserSettingsHandlers
{
    public class UpdateUserSettingsHandler : IRequestHandler<UpdateUserSettingsCommand>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly ILogger<GetUserSettingsHandler> _logger;

        public UpdateUserSettingsHandler(IUserSettingsService userSettingsService, IHttpService httpService,
            ILogger<GetUserSettingsHandler> logger)
        {
            _userSettingsService = userSettingsService;
            _httpService = httpService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpService.GetUserId());

            await _userSettingsService.UpdateUserSettingsAsync(currentUserId, request, cancellationToken);

            return Unit.Value;
        }
    }
}