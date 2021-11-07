using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.UserSettings.Commands
{
    public class UpdateUserSettingsTests : IntegrationTest
    {
        [Fact]
        public async Task ShouldCreateUserSettingsIfNotCreated()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var command = new UpdateUserSettingsCommand()
            {
                FirstName = "John",
                LastName = "Smith",
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

            var request = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var response = await _client.SendAsync(request);

            //assert that user settings have been added to the database
            var context = DbHelper.GetDbContext(_factory);
            var userSettings = await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            userSettings.Id.Should().NotBeEmpty();
            userSettings.UserId.Should().Be(userId);
            userSettings.FirstName.Should().Be(command.FirstName);
            userSettings.LastName.Should().Be(command.LastName);
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
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // First, create settings
            var command = new UpdateUserSettingsCommand()
            {
                FirstName = "John",
                LastName = "Smith",
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

            var request = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var createdSettingsResponse = await _client.SendAsync(request);
            
            // Update settings
            command = new UpdateUserSettingsCommand()
            {
                FirstName = "John",
                LastName = "Smith",
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

            request = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var updatedSettingsResponse = await _client.SendAsync(request);
            
            // Assert updated user settings have been added to the database
            var context = DbHelper.GetDbContext(_factory);
            var userSettings = await context.UserSettings.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId));

            createdSettingsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            updatedSettingsResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            userSettings.Id.Should().NotBeEmpty();
            userSettings.UserId.Should().Be(userId);
            userSettings.FirstName.Should().Be(command.FirstName);
            userSettings.LastName.Should().Be(command.LastName);
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