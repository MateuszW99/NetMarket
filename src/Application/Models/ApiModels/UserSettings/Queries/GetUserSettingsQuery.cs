using Application.Models.DTOs;
using MediatR;

namespace Application.Models.ApiModels.UserSettings.Queries
{
    public class GetUserSettingsQuery : IRequest<UserSettingsObject>
    {
    }
}