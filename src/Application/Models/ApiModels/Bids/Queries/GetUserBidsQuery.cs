using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Queries
{
    public class GetUserBidsQuery: IRequest<PaginatedList<BidObject>>
    {
        public int PageIndex { get; init; } 
        public int PageSize { get; init; } 
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