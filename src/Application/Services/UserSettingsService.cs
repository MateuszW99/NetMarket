using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly IApplicationDbContext _context;

        public UserSettingsService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserSettings> GetUserSettingsAsync(Guid userId)
        {
            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId) ?? new UserSettings()
            {
                UserId = userId,
                PaypalEmail = string.Empty,

                BillingStreet = string.Empty,
                BillingAddressLine1 = string.Empty,
                BillingAddressLine2 = string.Empty,
                BillingZipCode = string.Empty,
                BillingCountry = string.Empty,

                ShippingStreet = string.Empty,
                ShippingAddressLine1 = string.Empty,
                ShippingAddressLine2 = string.Empty,
                ShippingZipCode = string.Empty,
                ShippingCountry = string.Empty,
            };

            return userSettings;
        }

        public async Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);

            if (userSettings != null)
            {
                userSettings.PaypalEmail = request.PaypalEmail;

                userSettings.BillingStreet = request.BillingStreet;
                userSettings.BillingAddressLine1 = request.BillingAddressLine1;
                userSettings.BillingAddressLine2 = request.BillingAddressLine2;
                userSettings.BillingZipCode = request.BillingZipCode;
                userSettings.BillingCountry = request.BillingCountry;

                userSettings.ShippingStreet = request.ShippingStreet;
                userSettings.ShippingAddressLine1 = request.ShippingAddressLine1;
                userSettings.ShippingAddressLine2 = request.ShippingAddressLine2;
                userSettings.ShippingZipCode = request.ShippingZipCode;
                userSettings.ShippingCountry = request.ShippingCountry;

                await _context.SaveChangesAsync(cancellationToken);
            }

            else
            {
                await CreateUserSettingsAsync(userId, request, cancellationToken);
            }
        }

        public async Task CreateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var userSettings = new UserSettings()
            {
                UserId = userId,
                SellerLevel = SellerLevel.Beginner,
                SalesCompleted = 0,
                PaypalEmail = request.PaypalEmail,

                BillingStreet = request.BillingStreet,
                BillingAddressLine1 = request.BillingAddressLine1,
                BillingAddressLine2 = request.BillingAddressLine2,
                BillingZipCode = request.BillingZipCode,
                BillingCountry = request.BillingCountry,

                ShippingStreet = request.ShippingStreet,
                ShippingAddressLine1 = request.ShippingAddressLine1,
                ShippingAddressLine2 = request.ShippingAddressLine2,
                ShippingZipCode = request.ShippingZipCode,
                ShippingCountry = request.ShippingCountry
            };

            await _context.UserSettings.AddAsync(userSettings, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}