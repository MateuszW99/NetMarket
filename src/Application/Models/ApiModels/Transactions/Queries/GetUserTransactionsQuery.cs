using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Queries
{
    public class GetUserTransactionsQuery : IRequest<PaginatedList<TransactionObject>>
    {
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public string SearchQuery { get; init; }
    }

    public class GetUserTransactionsQueryValidator : AbstractValidator<GetUserTransactionsQuery>
    {
        public GetUserTransactionsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(10);
            RuleFor(x => x.SearchQuery).MaximumLength(100);
        }
    }
}