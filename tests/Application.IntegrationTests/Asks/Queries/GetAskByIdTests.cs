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
    public class GetAskByIdTests : IntegrationTest
    {
        private readonly IObjectDeserializer<AskObject> _askDeserializer;

        public GetAskByIdTests()
        {
            _askDeserializer = new AskObjectDeserializer();
        }

        [Fact]
        public async Task UserShouldGetTheirsOwnAskByItsId()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // Create ask
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                Size = "14",
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createAskRequest);

            var askFromDb = await context.Asks
                .AsNoTracking()
                .Include(x => x.Size)
                .Include(x => x.Item)
                .FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            // Get ask
            var getAskRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Asks}/{askFromDb.Id.ToString()}", UriKind.Relative));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(getAskRequest);
            var ask = _askDeserializer.Deserialize(response.Content);

            ask.Should().NotBeNull();
            ask.Id.Should().Be(askFromDb.Id.ToString());
            ask.Size.Id.Should().Be(askFromDb.Size.Id.ToString());
            ask.Item.Id.Should().Be(askFromDb.Item.Id.ToString());
            ask.Price.Should().Be(askFromDb.Price.ToString(CultureInfo.InvariantCulture));
            ask.UserId.Should().Be(askFromDb.CreatedBy.ToString()).And.Be(userId);
        }

        [Fact]
        public async Task UserShouldGetNotFoundWhenRequestingNotExistingAsk()
        {
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            var getAskRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Asks}/{Guid.NewGuid().ToString()}", UriKind.Relative));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            var response = await _client.SendAsync(getAskRequest);

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task OtherUserShouldNotGetFirstUsersAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);
            
            // FirstUser creates ask
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                Size = "14",
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            var createAskRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            createAskRequest.Content = new StringContent(JsonConvert.SerializeObject(createAskCommand), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);
            await _client.SendAsync(createAskRequest);
            
            var askFromDb = await context.Asks.AsNoTracking().FirstOrDefaultAsync(x => x.CreatedBy == Guid.Parse(userId));
            
            // OtherUser tries to get FirstUser's ask
            var otherUserId = await AuthHelper.RunAsOtherUserAsync(_factory);
            var otherUserAuthResult = _identityService.LoginAsync(OtherUser.Email, OtherUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", otherUserAuthResult.Result.Token);
            var getAskRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{Address.ApiBase}/{Address.Asks}/{askFromDb.Id.ToString()}", UriKind.Relative));
            var response = await _client.SendAsync(getAskRequest);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
    }
}