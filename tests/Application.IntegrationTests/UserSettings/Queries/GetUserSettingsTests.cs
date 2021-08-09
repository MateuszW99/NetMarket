using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.UserSettings.Commands;
using Application.Models.ApiModels.UserSettings.Queries;
using Application.Models.DTOs;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.UserSettings.Queries
{
    public class GetUserSettingsTests : IntegrationTest
    {
        [Fact]
        public async Task ShouldReturnEmptyUserSettingsObjectWhenUserHasNotCreatedTheSettings()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            var userSettings = await _mediator.Send(new GetUserSettingsQuery());

            userSettings.Should().BeOfType<UserSettingsObject>();

            userSettings.UserId.Should().Be(userId);
            userSettings.SellerLevel.Should().Be(SellerLevel.Beginner.ToString());
            userSettings.SalesCompleted.Should().Be(0);
            userSettings.PaypalEmail.Should().Be(string.Empty);
            userSettings.BillingStreet.Should().Be(string.Empty);
            userSettings.BillingAddressLine1.Should().Be(string.Empty);
            userSettings.BillingAddressLine2.Should().Be(string.Empty);
            userSettings.BillingZipCode.Should().Be(string.Empty);
            userSettings.BillingCountry.Should().Be(string.Empty);
            userSettings.ShippingStreet.Should().Be(string.Empty);
            userSettings.ShippingAddressLine1.Should().Be(string.Empty);
            userSettings.ShippingAddressLine2.Should().Be(string.Empty);
            userSettings.ShippingZipCode.Should().Be(string.Empty);
            userSettings.ShippingCountry.Should().Be(string.Empty);
        }

        [Fact]
        public async Task ShouldReturnNonEmptyUserSettingsObjectWhenUserHasCreatedTheSettings()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            //First, create user settings
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

            //assert that GetUserSettingsQuery returns correct user settings
            var userSettings = await _mediator.Send(new GetUserSettingsQuery());
            
            userSettings.Should().BeOfType<UserSettingsObject>();

            userSettings.UserId.Should().Be(userId);
            userSettings.SellerLevel.Should().Be(SellerLevel.Beginner.ToString());
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
        }
    }
}