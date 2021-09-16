using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.Asks.Commands;
using Application.Models.ApiModels.Bids.Commands;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Bids.Queries
{
    public class GetUserBidsTests : IntegrationTest
    {
        private readonly IObjectDeserializer<BidObject> _bidDeserializer;

        public GetUserBidsTests()
        {
            _bidDeserializer = new BidObjectDeserializer();
        }
        
        [Fact]
        public async Task UserShouldGetTheirsOwnNotEmptyBidListIfAnyBidExists()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 20;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);

            // Create multiple bids
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var numberOfBids = 5;
            for (int i = 1; i <= numberOfBids; i++)
            {
                price *= i;
                
                var createBidCommand = new CreateBidCommand()
                {
                    ItemId = item.Id.ToString(),
                    SizeId = size.Id.ToString(),
                    Price = price.ToString(CultureInfo.InvariantCulture)
                };
                
                var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
                createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
                await _client.SendAsync(createBidRequest);
            }
            
            // Get list of user's bids
            var bidListRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            var response = await _client.SendAsync(bidListRequest);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Should().NotBeNull();
            var bids = _bidDeserializer.DeserializeToList(response.Content);

            bids.Should().NotBeNull();
            bids.Count.Should().Be(numberOfBids);
            bids.TrueForAll(x => x.UserId == userId);
        }
        
        [Fact]
        public async Task UserShouldGetEmptyBidListIfTheyHaveNone()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            
            // Get list of user's bids
            var bidListRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            var response = await _client.SendAsync(bidListRequest);
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            response.Content.Should().NotBeNull();
            var bids = _bidDeserializer.DeserializeToList(response.Content);

            bids.Should().NotBeNull();
            bids.Count.Should().Be(0);
        }
    }
}