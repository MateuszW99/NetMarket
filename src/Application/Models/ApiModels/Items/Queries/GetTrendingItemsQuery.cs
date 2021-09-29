using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetTrendingItemsQuery : IRequest<List<ItemCard>>
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }
    
        public class GetTrendingItemsQueryValidator : AbstractValidator<GetTrendingItemsQuery>
        {
            public GetTrendingItemsQueryValidator()
            {
                RuleFor(x => x.Category).NotNull().NotEmpty();
                RuleFor(x => x.Count).GreaterThan(1);
            }
        }
}
