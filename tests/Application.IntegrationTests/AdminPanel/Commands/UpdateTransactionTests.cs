using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Transactions.Commands;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Application.IntegrationTests.AdminPanel.Commands
{
    public class UpdateTransactionTests : IntegrationTest
    {
        [Fact]
        public async Task AdministratorShouldUpdateTransaction()
        {
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);

            var context = DbHelper.GetDbContext(_factory);
            var oldTransaction = await context.Transactions.FirstOrDefaultAsync();
            var transactionId = oldTransaction.Id;

            var command = new UpdateTransactionCommand()
            {
                Id = transactionId.ToString(),
                Status = TransactionStatus.PayoutSend.ToString(),
                SellerFee = 15M,
                BuyerFee = 200M,
                Payout = 195M
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}/{transactionId}", UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var updatedTransaction = await DbHelper.FindAsync<Transaction>(_factory, transactionId);

            updatedTransaction.Status.Should()
                .Be((TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true));
            updatedTransaction.AskId.Should().Be(oldTransaction.AskId);
            updatedTransaction.BidId.Should().Be(oldTransaction.BidId);
            updatedTransaction.StartDate.Should().Be(oldTransaction.StartDate);
            updatedTransaction.EndDate.Should().Be(oldTransaction.EndDate);
            updatedTransaction.SellerFee.Should().Be(command.SellerFee);
            updatedTransaction.BuyerFee.Should().Be(command.BuyerFee);
            updatedTransaction.Payout.Should().Be(command.Payout);
            updatedTransaction.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            updatedTransaction.LastModifiedBy.Should().Be(Guid.Parse(userId));
        }

        [Fact]
        public async Task
            SupervisorShouldNotUpdateTransaction() //Supervisor can only change transaction status in supervisor panel
        {
            await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var command = new UpdateTransactionCommand();

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}/{Guid.NewGuid()}", UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task DefaultUserShouldNotUpdateTransaction() 
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            var authResult = await _identityService.LoginAsync(DefaultUser.Email, DefaultUser.Password);

            var command = new UpdateTransactionCommand();

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}/{Guid.NewGuid()}", UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task NotLoggedInUserShouldNotUpdateTransaction() 
        {
            var command = new UpdateTransactionCommand();

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}/{Guid.NewGuid()}", UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}