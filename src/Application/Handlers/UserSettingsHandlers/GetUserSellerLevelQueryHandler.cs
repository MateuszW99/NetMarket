using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Queries;
using MediatR;

namespace Application.Handlers.UserSettingsHandlers
{
    public class GetUserSellerLevelQueryHandler : IRequestHandler<GetUserSellerLevelQuery, string>
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly IHttpService _httpService;

        public GetUserSellerLevelQueryHandler(IUserSettingsService userSettingsService, IHttpService httpService)
        {
            _userSettingsService = userSettingsService;
            _httpService = httpService;
        }

        public async Task<string> Handle(GetUserSellerLevelQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var sellerLevel = await _userSettingsService.GetUserSellerLevel(Guid.Parse(userId));
            return sellerLevel.ToString();
        }
    }
}