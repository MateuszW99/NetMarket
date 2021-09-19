using Application.Common.Validators;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Queries
{
    public class GetBidByIdQuery : IRequest<BidObject>
    {
        public string Id { get; set; }
    }
    
    public class GetBidByIdQueryValidator : AbstractValidator<GetBidByIdQuery>
    {
        public GetBidByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
        }
    }
    
}