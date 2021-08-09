using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Commands;
using MediatR;

namespace Application.Handlers.UserSettingsHandlers
{
    public class UpdateUserSettingsQueryHandler : IRequestHandler<UpdateUserSettingsCommand>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;

        public UpdateUserSettingsQueryHandler(IUserSettingsService userSettingsService, IHttpService httpService)
        {
            _userSettingsService = userSettingsService;
            _httpService = httpService;
        }

        public async Task<Unit> Handle(UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpService.GetUserId());

            await _userSettingsService.UpdateUserSettingsAsync(currentUserId, request, cancellationToken);

            return Unit.Value;
        }
    }
}