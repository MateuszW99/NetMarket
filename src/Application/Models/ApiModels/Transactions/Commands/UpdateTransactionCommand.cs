using System;
using Application.Common.Validators;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class UpdateTransactionCommand : IRequest
    {
        public string Id { get; set; }
        public string AskId { get; set; }
        public string BidId { get; set; }
        public string Status { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
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

            RuleFor(x => x.AskId).IdMustMatchGuidPattern();
            RuleFor(x => x.BidId).IdMustMatchGuidPattern();
            RuleFor(x => x.Status).NotEmpty().IsEnumName(typeof(TransactionStatus));
            RuleFor(x => DateTime.ParseExact(x.EndDate, "MM/dd/yyyy", null))
                .GreaterThan(x => DateTime.ParseExact(x.StartDate, "MM/dd/yyyy", null))
                .When(x => !string.IsNullOrEmpty(x.EndDate));
            RuleFor(x => x.SellerFee).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.BuyerFee).NotEmpty().GreaterThanOrEqualTo(0);
            RuleFor(x => x.Payout).NotEmpty().GreaterThanOrEqualTo(0);
        }
    }
}