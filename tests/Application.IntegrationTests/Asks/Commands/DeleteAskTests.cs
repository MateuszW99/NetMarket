using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Asks.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Asks.Commands
{
    public class DeleteAskTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldDeleteTheirsOwnAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // Create ask
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = "200"
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createAskRequest);

            var ask = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            // Delete ask
            var deleteAskResponse = await _client.DeleteAsync(new Uri($"{Address.ApiBase}/{Address.Asks}/{ask.Id.ToString()}", UriKind.Relative));
            var deletedAsk = await context.Asks.FirstOrDefaultAsync(x => x.Id == ask.Id);
            
            deleteAskResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            deletedAsk.Should().BeNull();
        }

        [Fact]
        public async Task OtherUserShouldNotDeleteFirstUsersAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            
            // Create ask
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = "200"
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createAskRequest);
            
            var ask = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Other user tries to delete someone's ask
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(OtherUser.Email, OtherUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            var deleteAskResponse = await _client.DeleteAsync(new Uri($"{Address.ApiBase}/{Address.Asks}/{ask.Id.ToString()}", UriKind.Relative));
            
            var deletedAsk = await context.Asks.FirstOrDefaultAsync(x => x.Id == ask.Id);

            deleteAskResponse.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            deletedAsk.Should().NotBeNull();
        }
    }
}