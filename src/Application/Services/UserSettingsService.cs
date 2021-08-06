using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Entities;
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
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (userSettings == null)
            {
                throw new NotFoundException($"Entity \"{nameof(UserSettings)}\" for (UserId: {userId}) was not found");
            }

            return userSettings;
        }

        public async Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);

            var firstTimeCreated = false;
            
            if (userSettings == null)
            {
                firstTimeCreated = true;
                userSettings = new UserSettings
                {
                    UserId = userId
                };
            }
            
            userSettings.SellerLevel = request.SellerLevel;
            userSettings.SalesCompleted = request.SalesCompleted;
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

            if (firstTimeCreated)
            {
                await _context.UserSettings.AddAsync(userSettings, cancellationToken);
            }
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}