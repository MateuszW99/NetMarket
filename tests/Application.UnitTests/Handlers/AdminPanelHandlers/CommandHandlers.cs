using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.TransactionHandlers;
using Application.Models.ApiModels.Transactions.Commands;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.AdminPanelHandlers
{
    public class CommandHandlers
    {
        [Fact]
        public async Task UpdateTransactionHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateTransactionCommand = new UpdateTransactionCommand()
            {
                Id = Guid.NewGuid().ToString(),
                AskId = Guid.NewGuid().ToString(),
                BidId = Guid.NewGuid().ToString(),
                Status = "Arrived",
                StartDate = "08/20/2021",
                EndDate = "08/25/2021",
                SellerFee = 5M,
                BuyerFee = 135M,
                Payout = 135M
            };

            var commandHandler = new UpdateTransactionCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateTransactionCommand, MediatR.Unit>(new List<UpdateTransactionCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateTransactionCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateTransactionCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(Transactions))]
        public async Task UpdateTransactionCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(
            string status,
            string startDate, string endDate, decimal sellerFee,
            decimal buyerFee, decimal payout)
        {
            var updateTransactionCommand = new UpdateTransactionCommand()
            {
                AskId = Guid.NewGuid().ToString(),
                BidId = Guid.NewGuid().ToString(),
                Status = status,
                StartDate = startDate,
                EndDate = endDate,
                SellerFee = sellerFee,
                BuyerFee = buyerFee,
                Payout = payout
            };

            var commandHandler = new UpdateTransactionCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateTransactionCommand, MediatR.Unit>(new List<UpdateTransactionCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateTransactionCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateTransactionCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        public static IEnumerable<object[]> Transactions => new List<object[]>
        {
            new object[]
            {
                "Delivered", "08/20/2021", "08/15/2021", 8M, 160M, 150M
            },
            new object[]
            {
                "test", "08/20/2021", "08/25/2021", -2M, 160M, 150M
            },
            new object[]
            {
                "Started", "08/20/2021", null, 8M, 160M, 150M
            },
            new object[]
            {
                "test", "08/20/2021", "08/25/2021", 8M, 160M, 150M
            },
            new object[]
            {
                "Checked", "08/20/2021", "08/25/2021", -8M, -160M, -150M
            },
           
        };
    }
}