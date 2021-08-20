using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Queries
{
    public class GetTransactionsQuery : IRequest<TransactionObject>
    {
        public SearchTransactionsQuery SearchTransactionsQuery { get; set; }
    }

    public class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
    {
        public GetTransactionsQueryValidator()
        {
            RuleFor(x => x.SearchTransactionsQuery.Status).IsInEnum();
            RuleFor(x => x.SearchTransactionsQuery.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.SearchTransactionsQuery.PageSize).GreaterThanOrEqualTo(10);
        }
    }
}