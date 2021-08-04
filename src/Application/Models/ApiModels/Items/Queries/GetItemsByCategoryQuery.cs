using FluentValidation;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemsByCategoryQuery
    {
        public string Category { get; set; }
    }

    internal class GetItemsByCategoryQueryValidator : AbstractValidator<GetItemsByCategoryQuery>
    {
        public GetItemsByCategoryQueryValidator()
        {
            RuleFor(x => x.Category)
                .NotNull()
                .NotEmpty()
                .MinimumLength(1);
        }
    }
}