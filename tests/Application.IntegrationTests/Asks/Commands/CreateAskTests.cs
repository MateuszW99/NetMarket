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
    public class CreateAskTests : IntegrationTest
    {
        [Fact]
        public async Task UserShouldAddNewAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
        
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            var oldAsksCount = await context.Asks.CountAsync();
            decimal price = 200;
            
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            var authResult = _identityService.LoginAsync(DefaultUser.Email, DefaultUser.Password);

            var command = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = price.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newAsksCount = await context.Asks.CountAsync();
            var ask = await context.Asks.FirstOrDefaultAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            newAsksCount.Should().Be(oldAsksCount + 1);
            ask.Should().NotBeNull();
            ask.ItemId.Should().Be(item.Id);
            ask.SizeId.Should().Be(size.Id);
            ask.Price.Should().Be(price);
            ask.CreatedBy.Should().Be(userId);
        }
        
        [Fact]
        public async Task AdministratorShouldNotAddNewAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
        
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var oldAsksCount = await context.Asks.CountAsync();
            
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);
            
            var command = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = price.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newAsksCount = await context.Asks.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newAsksCount.Should().Be(oldAsksCount);
        }
        
        [Fact]
        public async Task SupervisorShouldNotAddNewAsk()
        {
            var context = DbHelper.GetDbContext(_factory);
        
            var size = await context.Sizes.FirstOrDefaultAsync();
            var item = await context.Items.FirstOrDefaultAsync();
            decimal price = 200;
            
            var oldAsksCount = await context.Asks.CountAsync();
            
            var userId = await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);
            
            var command = new CreateAskCommand()
            {
                ItemId = item.Id.ToString(),
                SizeId = size.Id.ToString(),
                Price = price.ToString()
            };

            var request = new HttpRequestMessage(HttpMethod.Post, new Uri($"{Address.ApiBase}/{Address.Asks}", UriKind.Relative));
            request.Content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Result.Token);

            var response = await _client.SendAsync(request);
            var newAsksCount = await context.Asks.CountAsync();
            
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
            newAsksCount.Should().Be(oldAsksCount);
        }
    }
}