using Application.Common.Models;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Supervisors.Queries
{
    public class GetSupervisorsQuery: IRequest<PaginatedList<SupervisorObject>>
    {        
        public int PageIndex { get; init; }
        public int PageSize { get; init; }
        public string SearchQuery { get; init; }
    }

    public class GetSupervisorsQueryValidator : AbstractValidator<GetSupervisorsQuery>
    {
        public GetSupervisorsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).GreaterThanOrEqualTo(10);
            RuleFor(x => x.SearchQuery).MaximumLength(100);
        }
    }
}