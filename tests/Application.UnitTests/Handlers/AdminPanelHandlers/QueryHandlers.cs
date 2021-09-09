using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Models;
using Application.Handlers.TransactionHandlers;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.AdminPanelHandlers
{
    public class QueryHandlers
    {
        [Fact]
        public async Task GetTransactionsQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery
                {
                    PageIndex = 1,
                    PageSize = 10
                }
            };

            var queryHandler = new GetTransactionsQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetTransactionsQuery, PaginatedList<TransactionObject>>(
                new List<GetTransactionsQueryValidator>
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query,
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }

        [Theory]
        [InlineData(-1, -1, null)]
        [InlineData(-1, 0, "Arrived")]
        [InlineData(10, 1, "test")]
        public async Task GetTransactionsQueryHandlerShouldThrowWhenPropertiesAreInvalid(int pageNumber, int pageSize,
            string status)
        {
            var query = new GetTransactionsQuery()
            {
                SearchTransactionsQuery = new SearchTransactionsQuery
                {
                    PageIndex = pageNumber,
                    PageSize = pageSize,
                    Status = status
                }
            };

            var queryHandler = new GetTransactionsQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetTransactionsQuery, PaginatedList<TransactionObject>>(
                new List<GetTransactionsQueryValidator>
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query,
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
    }
}