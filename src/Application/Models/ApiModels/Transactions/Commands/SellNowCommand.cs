using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class SellNowCommand : IRequest
    {
        public string BidId { get; set; }
        public string SellerFee { get; set; }
    }

    public class SellNowCommandValidator : AbstractValidator<SellNowCommand>
    {
        public SellNowCommandValidator()
        {
            RuleFor(x => x.BidId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => decimal.Parse(x.SellerFee)).GreaterThanOrEqualTo((decimal) 0.0);
        }
    }
}