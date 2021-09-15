using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemsWithCategoryQuery : IRequest<PaginatedList<ItemObject>>
    {
        public string Category { get; set; }    
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
    
    public class GetItemsWithCategoryQueryValidator : AbstractValidator<GetItemsWithCategoryQuery>
    {
        public GetItemsWithCategoryQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(10);

            RuleFor(x => x.Category).NotNull().NotEmpty();
        }
    }
}