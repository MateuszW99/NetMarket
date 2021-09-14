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

namespace Application.IntegrationTests.SupervisorPanel.Commands
{
    public class UpdateTransactionStatusTests : IntegrationTest
    {
        [Fact]
        public async Task SupervisorShouldUpdateTransactionStatus()
        {
            var userId = await AuthHelper.RunAsSupervisorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var context = DbHelper.GetDbContext(_factory);
            var oldTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.AssignedSupervisorId == Guid.Parse(userId));
            var transactionId = oldTransaction.Id;

            var command = new UpdateTransactionStatusCommand()
            {
                Id = transactionId.ToString(),
                Status = TransactionStatus.PayoutSend.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{transactionId.ToString()}",
                    UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);

            var updatedTransaction = await DbHelper.FindAsync<Transaction>(_factory, transactionId);

            updatedTransaction.Status.Should()
                .Be((TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true));
            updatedTransaction.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            updatedTransaction.LastModifiedBy.Should().Be(Guid.Parse(userId));
        }

        [Fact]
        public async Task ShouldThrowNotFoundWhenTransactionDoesNotExists()
        {
            await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var command = new UpdateTransactionStatusCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Status = TransactionStatus.PayoutSend.ToString(),
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{command.Id}",
                    UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            
        }

        [Fact]
        public async Task AdministratorShouldNotUpdateTransactionStatusFromSupervisorPanel()
        {
            var assignedSupervisorId = await AuthHelper.RunAsSupervisorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);

            await AuthHelper.RunAsAdministratorAsync(_factory);
            var authResult = await _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);

            var context = DbHelper.GetDbContext(_factory);
            var oldTransaction = await context.Transactions.FirstOrDefaultAsync(x => x.AssignedSupervisorId == Guid.Parse(assignedSupervisorId));
            var transactionId = oldTransaction.Id.ToString();
            
            var command = new UpdateTransactionStatusCommand()
            {
                Id = transactionId,
                Status = "Arrived"
            };

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{transactionId}",
                    UriKind.Relative));
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

            var command = new UpdateTransactionStatusCommand();

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{Guid.NewGuid().ToString()}",
                    UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task NotLoggedInUserShouldNotUpdateTransaction()
        {
            var command = new UpdateTransactionStatusCommand();

            var request = new HttpRequestMessage(HttpMethod.Put,
                new Uri($"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{Guid.NewGuid().ToString()}",
                    UriKind.Relative));
            request.Content =
                new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}