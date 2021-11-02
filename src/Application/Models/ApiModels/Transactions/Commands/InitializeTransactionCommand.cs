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
                .IdMustMatchGuidPattern().When(x => 
                    !string.IsNullOrEmpty(x.AskId) &&
                    string.IsNullOrEmpty(x.BidId));

            RuleFor(x => x.BidId)
                .IdMustMatchGuidPattern().When(x => 
                    !string.IsNullOrEmpty(x.BidId) &&
                    string.IsNullOrEmpty(x.AskId));
        }
    }
}