using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.BidHandlers;
using Application.Models.ApiModels.Bids.Commands;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Xunit;

namespace Application.UnitTests.Handlers.BidHandlers
{
    public class CommandHandlers
    {
        public static IEnumerable<object[]> Data => new[]
        {
            new[] { string.Empty, string.Empty, "0", string.Empty },
            new[] { string.Empty, Guid.NewGuid().ToString(), "0", string.Empty },
            new[] { null, string.Empty, "100", "14" }
        };
        
        #region CreateBidCommandHandler

        [Fact]
        public async Task CreateBidCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = Guid.NewGuid().ToString(),
                Price = "100.50",
                Size = "14"
            };

            var commandHandler = new CreateBidCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<CreateBidCommand, Unit>(
                new List<CreateBidCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(createBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(createBidCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task CreateBidCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id, string itemId, string price, string size)
        {
            var createBidCommand = new CreateBidCommand()
            {
                ItemId = itemId,
                Price = price,
                Size = size
            };

            var commandHandler = new CreateBidCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<CreateBidCommand, Unit>(
                new List<CreateBidCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(createBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(createBidCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion

        #region UpdateBidCommandHandler

        [Fact]
        public async Task UpdateAskCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateBidCommand = new UpdateBidCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Price = "100.50",
                Size = "14"
            };

            var commandHandler = new UpdateBidCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateBidCommand, Unit>(
                new List<UpdateBidCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(updateBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(updateBidCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task UpdateBidCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id, string itemId, string price, string size)
        {
            var updateBidCommand = new UpdateBidCommand()
            {
                Id = id,
                Price = price,
                Size = size
            };
        
            var commandHandler = new UpdateBidCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateBidCommand, Unit>(new List<UpdateBidCommandValidator>()
                {
                    new()
                }, null);
        
            await FluentActions.Invoking(() => validationBehaviour.Handle(updateBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(updateBidCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion

        #region DeleteBidCommandHandler

        [Fact]
        public async Task DeleteBidCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var deleteBidCommand = new DeleteBidCommand()
            {
                Id = Guid.NewGuid().ToString()
            };

            var commandHandler = new DeleteBidCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteBidCommand, Unit>(
                new List<DeleteBidCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(deleteBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(deleteBidCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc123")]
        public async Task DeleteBidCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id)
        {
            var deleteBidCommand = new DeleteBidCommand()
            {
                Id = id
            };
        
            var commandHandler = new DeleteBidCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteBidCommand, Unit>(new List<DeleteBidCommandValidator>()
                {
                    new()
                }, null);
        
            await FluentActions.Invoking(() => validationBehaviour.Handle(deleteBidCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(deleteBidCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion
    }
}