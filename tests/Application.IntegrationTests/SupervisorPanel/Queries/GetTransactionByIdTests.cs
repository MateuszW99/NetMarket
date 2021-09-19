using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.IntegrationTests.SupervisorPanel.Queries
{
    public class GetTransactionByIdTests: IntegrationTest
    {
        private readonly IObjectDeserializer<TransactionObject> _transactionObjectDeserializer;

        public GetTransactionByIdTests()
        {
            _transactionObjectDeserializer = new TransactionObjectDeserializer();
        }
        
        [Fact]
        public async Task ShouldReturnTransactionObject()
        {
            var supervisorId = await AuthHelper.RunAsSupervisorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var context = DbHelper.GetDbContext(_factory);

            var seededTransaction =
                await context.Transactions.FirstOrDefaultAsync(x => x.AssignedSupervisorId == Guid.Parse(supervisorId));
            
            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{seededTransaction.Id.ToString()}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            var transaction = _transactionObjectDeserializer.Deserialize(response.Content);

            transaction.Should().BeOfType<TransactionObject>();
            transaction.Should().NotBeNull();
            transaction.Ask.Should().Be(seededTransaction.Ask);
            transaction.Bid.Should().Be(seededTransaction.Bid);
            transaction.AssignedSupervisorId.Should().Be(seededTransaction.AssignedSupervisorId.ToString());
            transaction.Status.Should().Be(seededTransaction.Status.ToString());
            transaction.StartDate.Should().Be(seededTransaction.StartDate.ToString());
            transaction.SellerFee.Should().Be(seededTransaction.SellerFee);
            transaction.BuyerFee.Should().Be(seededTransaction.BuyerFee);
            transaction.Payout.Should().Be(seededTransaction.Payout);
        }
        
        [Fact]
        public async Task SupervisorShouldNotGetTransactionNotAssignedToHim()
        {
            var supervisorId = await AuthHelper.RunAsSupervisorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var context = DbHelper.GetDbContext(_factory);

            var seededTransaction =
                await context.Transactions.FirstOrDefaultAsync(x => x.AssignedSupervisorId != Guid.Parse(supervisorId));
            
            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}/{seededTransaction.Id.ToString()}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
    }
}