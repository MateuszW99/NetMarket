using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.UserSettings.Commands;
using Application.Models.ApiModels.UserSettings.Queries;
using Application.Models.DTOs;
using Domain.Enums;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.UserSettings.Queries
{
    public class GetUserSettingsTests : IntegrationTest
    {
        private readonly IObjectDeserializer<UserSettingsObject> _userSettingsDeserializer;
        
        public GetUserSettingsTests()
        {
            _userSettingsDeserializer = new UserSettingsObjectDeserializer();
        }
        
        [Fact]
        public async Task ShouldReturnEmptyUserSettingsObjectWhenUserHasNotCreatedTheSettings()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(new GetUserSettingsQuery()), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            var response = await _client.SendAsync(request);
            var userSettings = _userSettingsDeserializer.Deserialize(response.Content);
            
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
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // First, create user settings
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
            
            var request = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(request);
            
            // Assert that GetUserSettingsQuery returns correct user settings
            request = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.UserSettings}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(new GetUserSettingsQuery()), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var userSettings = _userSettingsDeserializer.Deserialize(response.Content);

            userSettings.Should().NotBeNull();
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