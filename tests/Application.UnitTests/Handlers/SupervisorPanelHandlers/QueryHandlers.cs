using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.TransactionHandlers;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.SupervisorPanelHandlers
{
    public class QueryHandlers
    {
        [Fact]
        public async Task GetTransactionByIdQueryShouldNotThrownWhenIdIsValid()
        {
            var query = new GetTransactionByIdQuery()
            {
                Id = Guid.NewGuid().ToString()
            };

            var queryHandler = new GetTransactionByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetTransactionByIdQuery, TransactionObject>(
                new List<GetTransactionByIdQueryValidator>
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
        [InlineData("")]
        [InlineData("1")]
        [InlineData("123-456-789-421")]
        public async Task GetTransactionByIdQueryShouldThrownWhenIdIsInvalid(string id)
        {
            var query = new GetTransactionByIdQuery()
            {
                Id = id
            };

            var queryHandler = new GetTransactionByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetTransactionByIdQuery, TransactionObject>(
                new List<GetTransactionByIdQueryValidator>
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