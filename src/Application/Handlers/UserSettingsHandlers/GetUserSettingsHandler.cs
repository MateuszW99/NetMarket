using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.UserSettingsHandlers
{
    public class GetUserSettingsHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsObject>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;
        private readonly ILogger<GetUserSettingsHandler> _logger;

        public GetUserSettingsHandler(IUserSettingsService userSettingsService, IHttpService httpService,
            IMapper mapper, ILogger<GetUserSettingsHandler> logger)
        {
            _userSettingsService = userSettingsService;
            _httpService = httpService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserSettingsObject> Handle(GetUserSettingsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = Guid.Parse(_httpService.GetUserId());

            var userSettings = await _userSettingsService.GetUserSettingsAsync(currentUserId);

            return _mapper.Map<UserSettingsObject>(userSettings);
        }
    }
}