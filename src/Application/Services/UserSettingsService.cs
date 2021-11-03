using System;
using System.Collections.Generic;
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

        private readonly IDictionary<SellerLevel, int> _salesNeededToUpdateSellerLevel = new Dictionary<SellerLevel, int>
        {
            { SellerLevel.Beginner, 3 },
            { SellerLevel.Intermediate, 10 },
            { SellerLevel.Advanced, 25 },
            { SellerLevel.Business, 100 }
        };

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
                FirstName = string.Empty,
                LastName = string.Empty,
                PaypalEmail = string.Empty,
                SellerLevel = SellerLevel.Beginner,
                SalesCompleted = 0,
                
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

        public async Task<SellerLevel> GetUserSellerLevel(Guid userId)
        {
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.CreatedBy == userId);
            return userSettings?.SellerLevel ?? SellerLevel.Beginner;
        }

        public async Task UpdateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request,
            CancellationToken cancellationToken)
        {
            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken: cancellationToken);

            if (userSettings == null)
            {
                await CreateUserSettingsAsync(userId, request, cancellationToken);
                return;
            }
            
            userSettings.FirstName = string.IsNullOrEmpty(request.FirstName) ? userSettings.FirstName : request.FirstName;
            userSettings.LastName = string.IsNullOrEmpty(request.LastName) ? userSettings.LastName : request.LastName;

            userSettings.PaypalEmail = string.IsNullOrEmpty(request.PaypalEmail) ? userSettings.PaypalEmail : request.PaypalEmail;

            userSettings.BillingStreet = string.IsNullOrEmpty(request.BillingStreet) ? userSettings.BillingStreet : request.BillingStreet;
            userSettings.BillingAddressLine1 = string.IsNullOrEmpty(request.BillingAddressLine1) ? userSettings.BillingAddressLine1 : request.BillingAddressLine1;
            userSettings.BillingAddressLine2 = request.BillingAddressLine2;
            userSettings.BillingZipCode = string.IsNullOrEmpty(request.BillingZipCode) ? userSettings.BillingZipCode : request.BillingZipCode;
            userSettings.BillingCountry = string.IsNullOrEmpty(request.BillingCountry) ? userSettings.BillingCountry : request.BillingCountry;
            
            userSettings.ShippingStreet = string.IsNullOrEmpty(request.ShippingStreet) ? userSettings.ShippingStreet : request.ShippingStreet;
            userSettings.ShippingAddressLine1 = string.IsNullOrEmpty(request.ShippingAddressLine1) ? userSettings.ShippingAddressLine1 : request.ShippingAddressLine1;
            userSettings.ShippingAddressLine2 = request.ShippingAddressLine2;
            userSettings.ShippingZipCode = string.IsNullOrEmpty(request.ShippingZipCode) ? userSettings.ShippingZipCode : request.ShippingZipCode;
            userSettings.ShippingCountry = string.IsNullOrEmpty(request.ShippingCountry) ? userSettings.ShippingCountry : request.ShippingCountry;
            
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateUserSettingsAsync(Guid userId, UpdateUserSettingsCommand request, CancellationToken cancellationToken)
        {
            var userSettings = new UserSettings()
            {
                UserId = userId,
                SellerLevel = SellerLevel.Beginner,
                SalesCompleted = 0,
                FirstName = request.FirstName,
                LastName = request.LastName,
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

        public async Task<bool> TryUpdateUserSellerLevel(Guid userId, CancellationToken cancellationToken)
        {
            var levelUpdated = false;
            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (userSettings is null)
            {
                return false;
            } 
            
            userSettings.SalesCompleted += 1;

            if (_salesNeededToUpdateSellerLevel[userSettings.SellerLevel] <= userSettings.SalesCompleted)
            {
                userSettings.SellerLevel++;
                levelUpdated = true;
            }
            await _context.SaveChangesAsync(cancellationToken);
            
            return levelUpdated;
        }
    }
}