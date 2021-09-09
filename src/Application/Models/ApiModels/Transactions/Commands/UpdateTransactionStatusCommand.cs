using Application.Common.Validators;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class UpdateTransactionStatusCommand : IRequest
    {
        public string Id { get; set; }
        public string Status { get; set; }
    }

    public class UpdateTransactionStatusCommandValidator : AbstractValidator<UpdateTransactionStatusCommand>
    {
        public UpdateTransactionStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().IdMustMatchGuidPattern();
            RuleFor(x => x.Status).NotEmpty().IsEnumName(typeof(TransactionStatus));
        }
    }
}