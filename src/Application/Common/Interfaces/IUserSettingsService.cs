using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetUserSettingsAsync(Guid userId);

        Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken);

        Task CreateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken);
    }
}