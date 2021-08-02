using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.ItemHandlers;
using Application.Models.ApiModels.Items.Commands;
using Application.UnitTests.Helpers;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.ItemHandlers
{
    [UseCulture("en-US")]
    public class CommandHandlers
    {
        public static IEnumerable<object[]> Items => new List<object[]>
        {
            new object[] { "", "", "", "", Convert.ToDecimal("0.0"), "", "", "", "" },
            new object[] { "af1", null, "", "", Convert.ToDecimal("0.0"), "", "wwww.google.com", "", "" },
            new object[] { "af1", "make", "model", "www.google.com", Convert.ToDecimal("1.20"), "", "", "", "" },
            new object[] { "", "", "", "", decimal.Zero, "", "", "", "" },
            new object[] { "af1", "make", "model", "description", Convert.ToDecimal("1.20"), "brand", "www.google.com", "www.google.com", "htpps://google.com" },
        };

        #region CreateItemCommandHandler

        [Fact]
        public async Task CreateItemCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var createItemCommand = new CreateItemCommand()
            {
                Name = "AF1 White",
                Make = "nike",
                Model = "air force 1",
                Description = "a short description",
                RetailPrice = decimal.One,
                ImageUrl = "www.google.com",
                SmallImageUrl = "www.google.com",
                ThumbUrl = "www.google.com",
                Brand = "Nike"
            };

            var commandHandler = new CreateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<CreateItemCommand, MediatR.Unit>(new List<CreateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        createItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(createItemCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Fact]
        public async Task CreateItemCommandHandlerShouldThrowWhenPropertiesAreNotInitialized()
        {
            var createItemCommand = new CreateItemCommand();
            var commandHandler = new CreateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<CreateItemCommand, MediatR.Unit>(new List<CreateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                validationBehaviour.Handle(
                    createItemCommand,
                    CancellationToken.None, 
                    () => commandHandler.Handle(createItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(Items))]
        public async Task CreateItemCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(
            string name, string make, string model,
            string description, decimal retailPrice, string brand,
            string imageUrl, string smallImageUrl, string thumbUrl)
        {
            var createItemCommand = new CreateItemCommand()
            {
                Name = name,
                Make = make,
                Model = model,
                Description = description,
                RetailPrice = retailPrice,
                ImageUrl = imageUrl,
                SmallImageUrl = smallImageUrl,
                ThumbUrl = thumbUrl,
                Brand = brand
            };

            var commandHandler = new CreateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<CreateItemCommand, MediatR.Unit>(new List<CreateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        createItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(createItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion
        
        #region DeleteItemCommandHandler

        [Fact]
        public async Task DeleteItemCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var deleteItemCommand = new DeleteItemCommand()
            {
                Id = Guid.NewGuid().ToString()
            };

            var commandHandler = new DeleteItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteItemCommand, MediatR.Unit>(new List<DeleteItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        deleteItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(deleteItemCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("xxxx")]
        [InlineData("{e24439407--14bf}")]
        [InlineData("{e24439e8-7b0b-4073-ba80-b14f99e314bf}")]
        [InlineData("(e24439e8-7b0b-4073-ba80-b14f99e314bf)")]
        [InlineData("e24439e87b0b4073ba80b14f99e314bf")]
        public async Task DeleteItemCommandHandlerShowThrowWhenPropertiesAreInvalid(string id)
        {
            var deleteItemCommand = new DeleteItemCommand()
            {
                Id = id
            };

            var commandHandler = new DeleteItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<DeleteItemCommand, MediatR.Unit>(new List<DeleteItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        deleteItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(deleteItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion

        #region UpdateItemCommandHandler

        [Fact]
        public async Task UpdateItemCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateItemCommand = new UpdateItemCommand()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "AF1 White",
                Make = "nike",
                Model = "air force 1",
                Description = "a short description",
                RetailPrice = decimal.One,
                ImageUrl = "www.google.com",
                SmallImageUrl = "www.google.com",
                ThumbUrl = "www.google.com",
                Brand = "Nike"
            };

            var commandHandler = new UpdateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateItemCommand, MediatR.Unit>(new List<UpdateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateItemCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Fact]
        public async Task UpdateItemCommandHandlerShouldThrowWhenPropertiesAreNotInitialized()
        {
            var updateItemCommand = new UpdateItemCommand();
            var commandHandler = new UpdateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateItemCommand, MediatR.Unit>(new List<UpdateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                validationBehaviour.Handle(
                    updateItemCommand,
                    CancellationToken.None, 
                    () => commandHandler.Handle(updateItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(Items))]
        public async Task UpdateItemCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(
            string name, string make, string model,
            string description, decimal retailPrice, string brand,
            string imageUrl, string smallImageUrl, string thumbUrl)
        {
            var updateItemCommand = new UpdateItemCommand()
            {
                Name = name,
                Make = make,
                Model = model,
                Description = description,
                RetailPrice = retailPrice,
                ImageUrl = imageUrl,
                SmallImageUrl = smallImageUrl,
                ThumbUrl = thumbUrl,
                Brand = brand
            };

            var commandHandler = new UpdateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<UpdateItemCommand, MediatR.Unit>(new List<UpdateItemCommandValidator>()
            {
                new()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(updateItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
    
        #endregion
    }
}