using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemsQuery : IRequest<PaginatedList<ItemObject>>
    {
        public SearchItemsQuery SearchQuery { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class GetItemsQueryValidator : AbstractValidator<GetItemsQuery>
    {
        public GetItemsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);

            RuleFor(x => x.SearchQuery).NotNull();
        }
    }
}