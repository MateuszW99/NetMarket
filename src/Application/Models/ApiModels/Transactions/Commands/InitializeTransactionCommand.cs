using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Commands
{
    public class InitializeTransactionCommand : IRequest
    {
        public string AskId { get; set; }
        public string BidId { get; set; }
    }

    public class InitializeTransactionCommandValidator : AbstractValidator<InitializeTransactionCommand>
    {
        public InitializeTransactionCommandValidator()
        {
            RuleFor(x => x.AskId)
                .NotNull()
                .IdMustMatchGuidPattern();
            
            RuleFor(x => x.BidId)
                .NotNull()
                .IdMustMatchGuidPattern();
        }
    }
}