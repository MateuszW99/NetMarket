using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class BuyNowCommand : IRequest
    {
        public string AskId { get; set; }
        public string BuyerFee { get; set; }
    }

    public class BuyNowCommandValidator : AbstractValidator<BuyNowCommand>
    {
        public BuyNowCommandValidator()
        {
            RuleFor(x => x.AskId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => decimal.Parse(x.BuyerFee)).GreaterThanOrEqualTo((decimal) 0.0);
        }
    }
}