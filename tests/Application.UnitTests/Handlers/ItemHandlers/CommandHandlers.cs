using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Handlers.ItemHandlers;
using Application.Models.ApiModels.Items.Commands;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.ItemHandlers
{
    public class CommandHandlers
    {
        [Fact]
        public async Task ShouldNotThrowWhenPropertiesAreValid()
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
                new CreateItemCommandValidator()
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
        public async Task ShouldThrowWhenPropertiesAreNotInitialized()
        {
            var createItemCommand = new CreateItemCommand();
            var commandHandler = new CreateItemCommandHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<CreateItemCommand, MediatR.Unit>(new List<CreateItemCommandValidator>()
            {
                new CreateItemCommandValidator()
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
        [MemberData(nameof(Data))]
        public async Task ShouldThrowWhenOneOrMorePropertiesAreInvalid(string name, string make, string model,
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
                new CreateItemCommandValidator()
            }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        createItemCommand,
                        CancellationToken.None, 
                        () => commandHandler.Handle(createItemCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[] { "", "", "", "", Convert.ToDecimal("0.0"), "", "", "", "" },
            new object[] { "af1", null, "", "", Convert.ToDecimal("0.0"), "", "wwww.google.com", "", "" },
            new object[] { "af1", "make", "model", "www.google.com", Convert.ToDecimal("1.20"), "", "", "", "" },
            new object[] { "", "", "", "", decimal.Zero, "", "", "", "" },
            new object[] { "af1", "make", "model", "description", Convert.ToDecimal("1.20"), "brand", "www.google.com", "www.google.com", "htpps://google.com" },
        };
    }
}