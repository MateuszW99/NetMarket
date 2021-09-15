using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Bids.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Bids.Commands
{
    public class UpdateBidTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldUpdateTheirsOwnBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var item = await context.Items.FirstOrDefaultAsync();
            var oldSize = await context.Sizes.FirstOrDefaultAsync();
            var newSize = await context.Sizes.LastOrDefaultAsync();
            decimal oldPrice = 200;
            decimal newPrice = 300;
            
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // Create bid
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = oldSize.Id.ToString(),
                Price = oldPrice.ToString()
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createBidRequest);
            
            var oldBid = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Update bid
            var updateBidCommand = new UpdateBidCommand()
            {
                Id = oldBid.Id.ToString(),
                Price = newPrice.ToString(),
                SizeId = newSize.Id.ToString()
            };
            var updateBidRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.Bids}/{oldBid.Id.ToString()}", UriKind.Relative));
            updateBidRequest.Content = new StringContent(JsonConvert.SerializeObject(updateBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            var updateBidResponse = await _client.SendAsync(updateBidRequest);
            
            var newBid = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId) && x.Id == oldBid.Id);
            
            updateBidResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            newBid.Should().NotBeNull();
            newBid.Price.Should().Be(newPrice).And.NotBe(oldPrice);
            newBid.SizeId.Should().Be(newSize.Id).And.NotBe(oldSize.Id);
            newBid.CreatedBy.Should().Be(oldBid.CreatedBy);
        }
        
        [Fact]
        public async Task OtherUserShouldNotUpdateFirstUsersBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var item = await context.Items.FirstOrDefaultAsync();
            var oldSize = await context.Sizes.FirstOrDefaultAsync();
            var newSize = await context.Sizes.LastOrDefaultAsync();
            decimal oldPrice = 200;
            decimal newPrice = 300;
            
            // Create bid
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = oldSize.Id.ToString(),
                Price = "200"
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createBidRequest);
            
            var oldBid = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Update bid
            var updateBidCommand = new UpdateBidCommand()
            {
                Id = oldBid.Id.ToString(),
                Price = newPrice.ToString(),
                SizeId = newSize.Id.ToString()
            };
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(OtherUser.Email, OtherUser.Password);
            var updateBidRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.Bids}/{oldBid.Id.ToString()}", UriKind.Relative));
            updateBidRequest.Content = new StringContent(JsonConvert.SerializeObject(updateBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            var updateBidResponse = await _client.SendAsync(updateBidRequest);

            var updatedBid = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.Id == oldBid.Id);

            updateBidResponse.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            updatedBid.Should().NotBeNull();
            updatedBid.Id.Should().Be(oldBid.Id);
            updatedBid.ItemId.Should().Be(oldBid.ItemId);
            updatedBid.SizeId.Should().Be(oldBid.SizeId);
            updatedBid.Price.Should().Be(oldBid.Price);
        }
    }
}