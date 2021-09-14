using Application.Common.Validators;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Queries
{
    public class GetTransactionByIdQuery : IRequest<TransactionObject>
    {
        public string Id { get; set; }
    }

    public class GetTransactionByIdQueryValidator : AbstractValidator<GetTransactionByIdQuery>
    {
        public GetTransactionByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().IdMustMatchGuidPattern();
        }
    }
}