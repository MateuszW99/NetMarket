using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.Asks.Commands;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Asks.Queries
{
    public class GetUsersAsksTests : IntegrationTest
    {
        private readonly IObjectDeserializer<AskObject> _askDeserializer;

        public GetUsersAsksTests()
        {
            _askDeserializer = new AskObjectDeserializer();
        }

        [Fact]
        public async Task UserShouldGetTheirsOwnNotEmptyAskListIfAnyAskExists()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 20;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);

            // Create multiple asks
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var numberOfAsks = 5;
            for (int i = 1; i <= numberOfAsks; i++)
            {
                price *= i;
                
                var createAskCommand = new CreateAskCommand()
                {
                    ItemId = item.Id.ToString(),
                    Size = "14",
                    Price = price.ToString(CultureInfo.InvariantCulture)
                };
                
                var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
                createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
                await _client.SendAsync(createAskRequest);
            }
            
            // Get list of user's asks
            var askListRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            var response = await _client.SendAsync(askListRequest);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Should().NotBeNull();
            var asks = _askDeserializer.DeserializeToList(response.Content);

            asks.Should().NotBeNull();
            asks.Count.Should().Be(numberOfAsks);
            asks.TrueForAll(x => x.UserId == userId);
        }

        [Fact]
        public async Task UserShouldGetEmptyAskListIfTheyHaveNone()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            // Get list of user's asks
            var askListRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            var response = await _client.SendAsync(askListRequest);
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Should().NotBeNull();
            var asks = _askDeserializer.DeserializeToList(response.Content);

            asks.Should().NotBeNull();
            asks.Count.Should().Be(0);
        }
    }
}