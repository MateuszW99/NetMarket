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

namespace Application.UnitTests.Handlers.SupervisorPanelHandlers
{
    public class CommandHandlers
    {
        [Fact]
        public async Task UpdateTransactionStatusHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateTransactionStatusCommand = new UpdateTransactionStatusCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Status = "Arrived",
            };

            var commandHandler = new UpdateTransactionStatusCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateTransactionStatusCommand, MediatR.Unit>(new List<UpdateTransactionStatusCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateTransactionStatusCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateTransactionStatusCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(Transactions))]
        public async Task UpdateTransactionStatusCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id, string status)
        {
            var updateTransactionStatusCommand = new UpdateTransactionStatusCommand()
            {
                Id = id,
                Status = status,
            };

            var commandHandler = new UpdateTransactionStatusCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateTransactionStatusCommand, MediatR.Unit>(new List<UpdateTransactionStatusCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateTransactionStatusCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateTransactionStatusCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        public static IEnumerable<object[]> Transactions => new List<object[]>
        {
            new object[]
            {
                "", "Delivered"
            },
            new object[]
            {
                "123", "Checked"
            },
            new object[]
            {
                "", "Delivered"
            },
            new object[]
            {
                Guid.NewGuid().ToString(), "test"
            },
        };
    }
}