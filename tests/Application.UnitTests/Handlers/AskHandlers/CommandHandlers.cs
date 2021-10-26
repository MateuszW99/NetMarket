using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.AskHandlers;
using Application.Models.ApiModels.Asks.Commands;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Xunit;

namespace Application.UnitTests.Handlers.AskHandlers
{
    public class CommandHandlers
    {
        public static IEnumerable<object[]> Data => new[]
        {
            new[] { string.Empty, string.Empty, "0", string.Empty },
            new[] { string.Empty, Guid.NewGuid().ToString(), "0", string.Empty },
            new[] { null, string.Empty, "100", "abc" }
        };
        
        
        #region CreateAskCommandHandler

        [Fact]
        public async Task CreateAskCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = Guid.NewGuid().ToString(),
                Price = "100.50",
                Size = "14"
            };

            var commandHandler = new CreateAskCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<CreateAskCommand, Unit>(
                new List<CreateAskCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(createAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(createAskCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task CreateAskCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id, string itemId, string price, string sizeId)
        {
            var createAskCommand = new CreateAskCommand()
            {
                ItemId = itemId,
                Price = price,
                Size = "14"
            };

            var commandHandler = new CreateAskCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<CreateAskCommand, Unit>(
                new List<CreateAskCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(createAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(createAskCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        
        
        #endregion

        #region UpdateAskCommandHandler

        [Fact]
        public async Task UpdateAskCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateAskCommand = new UpdateAskCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Price = "100.50",
                Size = "14"
            };

            var commandHandler = new UpdateAskCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateAskCommand, Unit>(
                new List<UpdateAskCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(updateAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(updateAskCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [MemberData(nameof(Data))]
        public async Task UpdateAskCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id, string itemId, string price, string sizeId)
        {
            var updateAskCommand = new UpdateAskCommand()
            {
                Id = id,
                Price = price,
                Size = "14"
            };

            var commandHandler = new UpdateAskCommandHandler(null, null, null, null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateAskCommand, Unit>(new List<UpdateAskCommandValidator>()
                {
                    new()
                }, null);
        
            await FluentActions.Invoking(() => validationBehaviour.Handle(updateAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(updateAskCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion

        #region DeleteAskCommandHandler

        [Fact]
        public async Task DeleteAskCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var deleteAskCommand = new DeleteAskCommand()
            {
                Id = Guid.NewGuid().ToString()
            };

            var commandHandler = new DeleteAskCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteAskCommand, Unit>(
                new List<DeleteAskCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(deleteAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(deleteAskCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("abc123")]
        public async Task DeleteAskCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(string id)
        {
            var deleteAskCommand = new DeleteAskCommand()
            {
                Id = id
            };
        
            var commandHandler = new DeleteAskCommandHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteAskCommand, Unit>(new List<DeleteAskCommandValidator>()
                {
                    new()
                }, null);
        
            await FluentActions.Invoking(() => validationBehaviour.Handle(deleteAskCommand,
                    CancellationToken.None,
                    () => commandHandler.Handle(deleteAskCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion
    }
}