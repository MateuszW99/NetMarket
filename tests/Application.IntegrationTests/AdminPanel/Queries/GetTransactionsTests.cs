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

namespace Application.IntegrationTests.AdminPanel.Queries
{
    public class GetTransactionsTests : IntegrationTest
    {
        private readonly IObjectDeserializer<TransactionObject> _transactionObjectDeserializer;

        public GetTransactionsTests()
        {
            _transactionObjectDeserializer = new TransactionObjectDeserializer();
        }

        [Fact]
        public async Task ShouldReturnTransactionObjects()
        {
            await AuthHelper.RunAsAdministratorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);

            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            var transactions = _transactionObjectDeserializer.DeserializeToList(response.Content);

            transactions.Should().AllBeOfType<TransactionObject>();
            transactions.Should().NotBeNullOrEmpty();
            transactions.Count.Should().Be(5); //5 transactions seeded
        }

        [Theory]
        [InlineData("Started", 2)]
        [InlineData("AwaitingSender", 0)]
        [InlineData("EnRouteFromSeller", 0)]
        [InlineData("Checked", 1)]
        [InlineData("PayoutSend", 0)]
        [InlineData("EnRouteFromWarehouse", 1)]
        [InlineData("Delivered", 1)]
        public async Task ShouldReturnCorrectTransactionObjectsCountForRequestsWithSpecifiedStatus(string status, int expectedCount)
        {
            await AuthHelper.RunAsAdministratorAsync(_factory);
            await TestTransactionsSeeder.SeedTestTransactionsAsync(_factory);
            var authResult = await _identityService.LoginAsync(AdminUser.Email, AdminUser.Password);

            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                    Status = status
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}" +
                    $"?status={query.SearchTransactionsQuery.Status}&pageIndex={query.SearchTransactionsQuery.PageIndex}" +
                    $"&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);
            var transactions = _transactionObjectDeserializer.DeserializeToList(response.Content);

            transactions.Should().AllBeOfType<TransactionObject>();
            transactions.Count.Should().Be(expectedCount);
        }

        [Fact]
        public async Task SupervisorShouldNotGetAllTransactions()
        {
            await AuthHelper.RunAsSupervisorAsync(_factory);
            var authResult = await _identityService.LoginAsync(SupervisorUser.Email, SupervisorUser.Password);

            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task DefaultUserShouldNotGetAllTransactions()
        {
            await AuthHelper.RunAsFirstUserAsync(_factory);
            var authResult = await _identityService.LoginAsync(FirstUser.Email, FirstUser.Password);

            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResult.Token);

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }
        
        [Fact]
        public async Task NotLoggedInUserShouldNotGetTransactions()
        {
            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery()
                {
                    PageIndex = 1,
                    PageSize = 10,
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"{Address.ApiBase}/{Address.AdminPanel}/{Address.Orders}" +
                    $"?pageIndex={query.SearchTransactionsQuery.PageIndex}&pageSize={query.SearchTransactionsQuery.PageSize}",
                    UriKind.Relative));

            var response = await _client.SendAsync(request);

            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }
    }
}