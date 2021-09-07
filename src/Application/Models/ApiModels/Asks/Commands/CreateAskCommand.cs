using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Asks.Commands
{
    public class CreateAskCommand : IRequest
    {
        public string ItemId { get; set; }
        public string SizeId { get; set; }
        public string Price { get; set; }
    }

    public class CreateAskCommandValidator : AbstractValidator<CreateAskCommand>
    {
        public CreateAskCommandValidator()
        {
            RuleFor(x => x.ItemId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.SizeId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}