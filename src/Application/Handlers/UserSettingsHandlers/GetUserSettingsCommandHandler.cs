using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.UserSettingsHandlers
{
    public class GetUserSettingsQueryHandler : IRequestHandler<GetUserSettingsQuery, UserSettingsObject>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;

        public GetUserSettingsQueryHandler(IUserSettingsService userSettingsService, IHttpService httpService,
            IMapper mapper)
        {
            _userSettingsService = userSettingsService;
            _httpService = httpService;
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