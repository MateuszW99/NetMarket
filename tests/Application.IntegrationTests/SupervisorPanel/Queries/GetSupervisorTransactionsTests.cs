using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Application.IntegrationTests.SupervisorPanel.Queries
{
    public class GetSupervisorTransactionsTests : IntegrationTest
    {
        private readonly IObjectDeserializer<TransactionObject> _transactionObjectDeserializer;

        public GetSupervisorTransactionsTests()
        {
            _transactionObjectDeserializer = new TransactionObjectDeserializer();
        }

        [Fact]
        public async Task SupervisorShouldGetAssignedTransactions()
        {
            await AuthHelper.RunAsSupervisorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var query = new GetSupervisorTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            var transactions = _transactionObjectDeserializer.DeserializeToList(response.Content);

            transactions.Should().AllBeOfType<TransactionObject>();
            transactions.Should().NotBeNullOrEmpty();
            transactions.Count.Should().Be(3); //3 assigned transactions seeded
        }

        [Fact]
        public async Task DefaultUserShouldNotGetTransactionsFromSupervisorPanel()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            var authResult = await _identityService.LoginAsync(DefaultUser.Email, DefaultUser.Password);

            var query = new GetSupervisorTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact]
        public async Task NotLoggedInUserShouldNotGetTransactionsFromSupervisorPanel()
        {
            var query = new GetSupervisorTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.SupervisorPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            var response = await _client.SendAsync(request);
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}