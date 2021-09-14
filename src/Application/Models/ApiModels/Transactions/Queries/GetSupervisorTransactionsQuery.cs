using Application.Common.Models;
using Application.Models.DTOs;
using Domain.Enums;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Transactions.Queries
{
    public class GetSupervisorTransactionsQuery : IRequest<PaginatedList<TransactionObject>>
    {
        public SearchTransactionsQuery SearchTransactionsQuery { get; set; }
    }

    public class GetSupervisorTransactionsQueryValidator : AbstractValidator<GetSupervisorTransactionsQuery>
    {
        public GetSupervisorTransactionsQueryValidator()
        {
            RuleFor(x => x.SearchTransactionsQuery.Status).IsEnumName(typeof(TransactionStatus));
            RuleFor(x => x.SearchTransactionsQuery.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.SearchTransactionsQuery.PageSize).GreaterThanOrEqualTo(10);
        }
    }
}