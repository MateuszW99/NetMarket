using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IUserSettingsService
    {
        Task<UserSettings> GetUserSettingsAsync(Guid userId);
        Task<SellerLevel> GetUserSellerLevel(Guid userId);
        Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request, CancellationToken cancellationToken);
        Task CreateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request, CancellationToken cancellationToken);
        Task<bool> TryUpdateUserSellerLevel(Guid userId, CancellationToken cancellationToken);
    }
}