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
    public class DeleteBidTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldDeleteTheirsOwnBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = "200"
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createBidRequest);

            var ask = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            var deleteBidResponse = await _client.DeleteAsync(new Uri($"{Address.ApiBase}/{Address.Bids}/{ask.Id.ToString()}", UriKind.Relative));
            var deletedAsk = await context.Asks.FirstOrDefaultAsync(x => x.Id == ask.Id);
            
            deleteBidResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            deletedAsk.Should().BeNull();
        }

        [Fact]
        public async Task FirstUserShouldNotDeleteOtherUsersBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            
            // Create bid
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = "200"
            };
            var createBidRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            createBidRequest.Content = new StringContent(JsonConvert.SerializeObject(createBidCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createBidRequest);
            
            var bid = await context.Bids.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Other user tries to delete someone's bid
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            var deleteAskResponse = await _client.DeleteAsync(new Uri($"{Address.ApiBase}/{Address.Bids}/{bid.Id.ToString()}", UriKind.Relative));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            
            var deletedAsk = await context.Bids.FirstOrDefaultAsync(x => x.Id == bid.Id);

            deleteAskResponse.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            deletedAsk.Should().NotBeNull();
        } 
    }
}