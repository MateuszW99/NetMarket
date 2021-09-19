using System;
using Application.Common.Validators;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Application.UnitTests.Common.Validators
{
    internal class Command
    {
        public string Id { get; set; }
        public string ZipCode { get; set; }
    }

    internal class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .IdMustMatchGuidPattern();

            RuleFor(x => x.ZipCode)
                .MustMatchZipCodePattern()
                .MaximumLength(6);
        }
    }


    public class CustomValidatorsTests
    {
        private readonly CommandValidator validator;

        public CustomValidatorsTests()
        {
            validator = new();
        }

        [Fact]
        public void ShouldValidateNewGuid()
        {
            var commandToValidate = new Command() { Id = Guid.NewGuid().ToString() };
            var validationResult = validator.TestValidate(commandToValidate);
            validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Fact]
        public void ShouldValidateAlreadyGeneratedGuid()
        {
            var commandToValidate = new Command() { Id = "73c13a85-caf8-45ae-9bea-d48a4aeb6b2c" };
            var validationResult = validator.TestValidate(commandToValidate);
            validationResult.ShouldNotHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("randomString")]
        [InlineData("18cc36cab7c54d38b11930d78ff3d433")]
        [InlineData("73c13a-f8-45ae-9be-d48a4aeb6b2c")]
        [InlineData("73c13a85 caf8 45ae 9bea d48a4aeb6b2c")]
        [InlineData("{73c13a85-caf8-45ae-9bea-d48a4aeb6b2c}")]
        public void ShouldNotValidateStringAsGuid(string valueToValidate)
        {
            var commandToValidate = new Command() { Id = valueToValidate };
            var validationResult = validator.TestValidate(commandToValidate);
            validationResult.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData("11-223")]
        [InlineData("11223")]
        [InlineData("12345")]
        [InlineData("12-345")]
        public void ShouldValidateCorrectZipCodes(string zipCode)
        {
            var commandToValidate = new Command() { ZipCode = zipCode };
            var validationResult = validator.TestValidate(commandToValidate);
            validationResult.ShouldNotHaveValidationErrorFor(x => x.ZipCode);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("1234")]
        [InlineData("12-3455")]
        [InlineData("123-345")]
        [InlineData("12344421345")]
        [InlineData("1-")]
        [InlineData("-123")]
        public void ShouldNotValidateIncorrectZipCodes(string zipCode)
        {
            var commandToValidate = new Command() { ZipCode = zipCode };
            var validationResult = validator.TestValidate(commandToValidate);
            validationResult.ShouldHaveValidationErrorFor(x => x.ZipCode);
        }
    }
}