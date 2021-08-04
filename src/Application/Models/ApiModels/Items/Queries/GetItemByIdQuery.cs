using Application.Common.Validators;
using FluentValidation;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemByIdQuery
    {
        public string Id { get; set; }
    }

    internal class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
    {
        public GetItemByIdQueryValidator()
        {
            RuleFor(x => x.Id).IdMustMatchGuidPattern();
        }
    }
}