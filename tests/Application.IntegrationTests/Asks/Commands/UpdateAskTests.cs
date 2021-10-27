using System;
using System.Globalization;
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
    public class UpdateAskTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldUpdateTheirsOwnAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
            var item = await context.Items.FirstOrDefaultAsync();
            var oldSize = await context.Sizes.FirstOrDefaultAsync();
            var newSize = await context.Sizes.LastOrDefaultAsync();
            decimal oldPrice = 200;
            decimal newPrice = 300;
            
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // Create ask
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                Size = "14",
                Price = oldPrice.ToString()
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createAskRequest);
            
            var oldAsk = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Update ask
            var updateAskCommand = new UpdateAskCommand()
            {
                Id = oldAsk.Id.ToString(),
                Price = newPrice.ToString(),
                Size = "14",
            };
            var updateAskRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.Asks}/{oldAsk.Id.ToString()}", UriKind.Relative));
            updateAskRequest.Content = new StringContent(JsonConvert.SerializeObject(updateAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            var updateAskResponse = await _client.SendAsync(updateAskRequest);
            
            var newAsk = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId) && x.Id == oldAsk.Id);
            
            updateAskResponse.StatusCode.Should().Be(StatusCodes.Status200OK);
            newAsk.Should().NotBeNull();
            newAsk.Price.Should().Be(newPrice).And.NotBe(oldPrice);
            newAsk.SizeId.Should().Be(newSize.Id).And.NotBe(oldSize.Id);
            newAsk.CreatedBy.Should().Be(oldAsk.CreatedBy);
        }

        [Fact]
        public async Task OtherUserShouldNotUpdateFirstUsersAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
            var item = await context.Items.FirstOrDefaultAsync();
            var oldSize = await context.Sizes.FirstOrDefaultAsync();
            var newSize = await context.Sizes.LastOrDefaultAsync();
            decimal oldPrice = 200;
            decimal newPrice = 300;
            
            // Create ask
            var firstUserId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var firstUserAuthResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                Size = oldSize.Value,
                Price = oldPrice.ToString(CultureInfo.InvariantCulture)
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", firstUserAuthResult.Result.Token);
            await _client.SendAsync(createAskRequest);
            
            var oldAsk = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(firstUserId));
            
            // Update ask
            var updateAskCommand = new UpdateAskCommand()
            {
                Id = oldAsk.Id.ToString(),
                Price = newPrice.ToString(),
                Size = newSize.Value
            };
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(OtherUser.Email, OtherUser.Password);
            var updateAskRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"{Address.ApiBase}/{Address.Asks}/{oldAsk.Id.ToString()}", UriKind.Relative));
            updateAskRequest.Content = new StringContent(JsonConvert.SerializeObject(updateAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            var updateAskResponse = await _client.SendAsync(updateAskRequest);

            var updatedAsk = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == oldAsk.Id);

            updateAskResponse.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            updatedAsk.Should().NotBeNull();
            updatedAsk.Id.Should().Be(oldAsk.Id);
            updatedAsk.ItemId.Should().Be(oldAsk.ItemId);
            updatedAsk.SizeId.Should().Be(oldAsk.SizeId);
            updatedAsk.Price.Should().Be(oldAsk.Price);
        }
    }
}