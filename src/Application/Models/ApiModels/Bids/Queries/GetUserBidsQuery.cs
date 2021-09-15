using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Queries
{
    public class GetUserBidsQuery: IRequest<PaginatedList<BidObject>>
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    
    public class GetUserBidsQueryValidator : AbstractValidator<GetUserBidsQuery>
    {
        public GetUserBidsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(10);
        }
    }
}