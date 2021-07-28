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
    }

    internal class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .NotNull()
                .IdMustMatchGuidPattern();
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
    }
}