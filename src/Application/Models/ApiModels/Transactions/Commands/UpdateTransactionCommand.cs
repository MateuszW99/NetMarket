using Application.Common.Validators;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class UpdateTransactionCommand : IRequest
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public decimal SellerFee { get; set; }
        public decimal BuyerFee { get; set; }
        public decimal Payout { get; set; }
    }

    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .IdMustMatchGuidPattern();
            
            RuleFor(x => x.Status).NotEmpty().IsEnumName(typeof(TransactionStatus));
            RuleFor(x => x.SellerFee).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.BuyerFee).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Payout).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}