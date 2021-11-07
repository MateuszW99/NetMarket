using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.Bids.Commands;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Bids.Queries
{
    public class GetBidByIdTests : IntegrationTest
    {
        private readonly IObjectDeserializer<BidObject> _bidDeserializer;

        public GetBidByIdTests()
        {
            _bidDeserializer = new BidObjectDeserializer();
        }
        
        [Fact]
        public async Task UserShouldGetTheirsOwnBidByItsId()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // Create bid
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                Size = size.Value,
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createBidRequest);

            var bidFromDb = await context.Bids
                .AsNoTracking()
                .Include(x => x.Size)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            // Get bid
            var getBidRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Bids}/{bidFromDb.Id.ToString()}", UriKind.Relative));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(getBidRequest);
            var bid = _bidDeserializer.Deserialize(response.Content);

            bid.Should().NotBeNull();
            bid.Id.Should().Be(bidFromDb.Id.ToString());
            bid.Size.Id.Should().Be(bidFromDb.Size.Id.ToString());
            bid.Item.Id.Should().Be(bidFromDb.Item.Id.ToString());
            bid.Price.Should().Be(bidFromDb.Price.ToString(CultureInfo.InvariantCulture));
            bid.UserId.Should().Be(bidFromDb.CreatedBy.ToString()).And.Be(userId);
        }

        [Fact]
        public async Task UserShouldGetNotFoundWhenRequestingNotExistingBid()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var getBidRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Bids}/{Guid.NewGuid().ToString()}", UriKind.Relative));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(getBidRequest);
            
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
        
                [Fact]
        public async Task OtherUserShouldNotGetFirstUsersBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // FirstUser creates bid
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                Size = size.Value,
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createBidRequest);
            
            var bidFromDb = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            // OtherUser tries to get FirstUser's bid
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(OtherUser.Email, OtherUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            var getBidRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Bids}/{bidFromDb.Id.ToString()}", UriKind.Relative));
            var response = await _client.SendAsync(getBidRequest);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
    }
}