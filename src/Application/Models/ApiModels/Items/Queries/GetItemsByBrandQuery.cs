using FluentValidation;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemsByBrandQuery
    {
        public string Brand { get; set; }
    }

    internal class GetItemsByBrandQueryValidator : AbstractValidator<GetItemsByBrandQuery>
    {
        public GetItemsByBrandQueryValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty()
                .NotNull()
                .MinimumLength(1);
        }
    }
}