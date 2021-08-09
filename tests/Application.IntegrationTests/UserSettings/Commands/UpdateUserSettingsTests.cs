using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.IntegrationTests.UserSettings.Commands
{
    public class UpdateUserSettingsTests : IntegrationTest
    {
        [Fact]
        public async Task ShouldCreateUserSettingsIfNotCreated()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            var command = new UpdateUserSettingsCommand()
            {
                PaypalEmail = "test@test.com",

                BillingStreet = "Test billing street",
                BillingAddressLine1 = "Test billing address line 1",
                BillingAddressLine2 = "Test billing address line 2",
                BillingZipCode = "12-345",
                BillingCountry = "USA",

                ShippingStreet = "Test shipping street",
                ShippingAddressLine1 = "Test shipping address line 1",
                ShippingAddressLine2 = "Test shipping address line 2",
                ShippingZipCode = "12-345",
                ShippingCountry = "USA"
            };

            await _mediator.Send(command);

            //assert that user settings have been added to the database
            var context = DbHelper.GetDbContext(_factory);
            var userSettings = await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            userSettings.Id.Should().NotBeEmpty();
            userSettings.UserId.Should().Be(userId);
            userSettings.SellerLevel.Should().Be(SellerLevel.Beginner);
            userSettings.SalesCompleted.Should().Be(0);
            userSettings.PaypalEmail.Should().Be(command.PaypalEmail);
            userSettings.BillingStreet.Should().Be(command.BillingStreet);
            userSettings.BillingAddressLine1.Should().Be(command.BillingAddressLine1);
            userSettings.BillingAddressLine2.Should().Be(command.BillingAddressLine2);
            userSettings.BillingZipCode.Should().Be(command.BillingZipCode);
            userSettings.BillingCountry.Should().Be(command.BillingCountry);
            userSettings.ShippingStreet.Should().Be(command.ShippingStreet);
            userSettings.ShippingAddressLine1.Should().Be(command.ShippingAddressLine1);
            userSettings.ShippingAddressLine2.Should().Be(command.ShippingAddressLine2);
            userSettings.ShippingZipCode.Should().Be(command.ShippingZipCode);
            userSettings.ShippingCountry.Should().Be(command.ShippingCountry);

            userSettings.CreatedBy.Should().Be(userId);
            userSettings.Created.Should().BeCloseTo(DateTime.Now, 1000);
            userSettings.LastModified.Should().BeNull();
            userSettings.LastModifiedBy.Should().BeNull();
        }

        [Fact]
        public async Task ShouldUpdateUserSettingsIfCreated()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            
            //First, create settings
            var command = new UpdateUserSettingsCommand()
            {
                PaypalEmail = "test@test.com",

                BillingStreet = "Test billing street",
                BillingAddressLine1 = "Test billing address line 1",
                BillingAddressLine2 = "Test billing address line 2",
                BillingZipCode = "12-345",
                BillingCountry = "USA",

                ShippingStreet = "Test shipping street",
                ShippingAddressLine1 = "Test shipping address line 1",
                ShippingAddressLine2 = "Test shipping address line 2",
                ShippingZipCode = "12-345",
                ShippingCountry = "USA"
            };

            await _mediator.Send(command);
            
            //Update settings
            command = new UpdateUserSettingsCommand()
            {
                PaypalEmail = "test@test.com",

                BillingStreet = "Updated billing street",
                BillingAddressLine1 = "Updated billing address line 1",
                BillingAddressLine2 = "Updated billing address line 2",
                BillingZipCode = "12-345",
                BillingCountry = "USA",

                ShippingStreet = "Updated shipping street",
                ShippingAddressLine1 = "Updated shipping address line 1",
                ShippingAddressLine2 = "Updated shipping address line 2",
                ShippingZipCode = "12-345",
                ShippingCountry = "USA"
            };
            
            await _mediator.Send(command);

            //assert that updated user settings have been added to the database
            var context = DbHelper.GetDbContext(_factory);
            var userSettings = await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            userSettings.Id.Should().NotBeEmpty();
            userSettings.UserId.Should().Be(userId);
            userSettings.SellerLevel.Should().Be(SellerLevel.Beginner);
            userSettings.SalesCompleted.Should().Be(0);
            userSettings.PaypalEmail.Should().Be(command.PaypalEmail);
            userSettings.BillingStreet.Should().Be(command.BillingStreet);
            userSettings.BillingAddressLine1.Should().Be(command.BillingAddressLine1);
            userSettings.BillingAddressLine2.Should().Be(command.BillingAddressLine2);
            userSettings.BillingZipCode.Should().Be(command.BillingZipCode);
            userSettings.BillingCountry.Should().Be(command.BillingCountry);
            userSettings.ShippingStreet.Should().Be(command.ShippingStreet);
            userSettings.ShippingAddressLine1.Should().Be(command.ShippingAddressLine1);
            userSettings.ShippingAddressLine2.Should().Be(command.ShippingAddressLine2);
            userSettings.ShippingZipCode.Should().Be(command.ShippingZipCode);
            userSettings.ShippingCountry.Should().Be(command.ShippingCountry);

            userSettings.CreatedBy.Should().Be(userId);
            userSettings.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            userSettings.LastModifiedBy.Should().Be(userId);
        }
    }
}