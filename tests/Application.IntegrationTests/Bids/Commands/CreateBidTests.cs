using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Asks.Commands;
using Application.Models.ApiModels.Bids.Commands;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.Bids.Commands
{
    public class CreateBidTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldAddNewBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            var oldBidsCount = await context.Bids.CountAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);

            var command = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = price.ToString()
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newBidsCount = await context.Bids.CountAsync();
            var bid = await context.Bids.FirstOrDefaultAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            newBidsCount.Should().Be(oldBidsCount + 1);
            bid.Should().NotBeNull();
            bid.ItemId.Should().Be(item.Id);
            bid.SizeId.Should().Be(size.Id);
            bid.Price.Should().Be(price);
            bid.CreatedBy.Should().Be(userId);
        }
        
        [Fact]
        public async Task AdministratorShouldNotAddNewBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var oldBidsCount = await context.Bids.CountAsync();
            
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);
            
            var command = new CreateBidCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = price.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newBidsCount = await context.Bids.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newBidsCount.Should().Be(oldBidsCount);
        }
        
        [Fact]
        public async Task SupervisorShouldNotAddNewBid()
        {
            var context = DbHelper.GetDbContext(_factory);
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            var oldBidsCount = await context.Bids.CountAsync();
            
            var userId = await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);
            
            var command = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                Size = "14",
                Price = price.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Bids}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newBidsCount = await context.Bids.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newBidsCount.Should().Be(oldBidsCount);
        }
    }
}