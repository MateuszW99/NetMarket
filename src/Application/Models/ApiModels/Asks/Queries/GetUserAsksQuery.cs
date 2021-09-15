using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Asks.Queries
{
    public class GetUserAsksQuery : IRequest<PaginatedList<AskObject>>
    {        
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

    public class GetUserAsksQueryValidator : AbstractValidator<GetUserAsksQuery>
    {
        public GetUserAsksQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(10);
        }
    }
}