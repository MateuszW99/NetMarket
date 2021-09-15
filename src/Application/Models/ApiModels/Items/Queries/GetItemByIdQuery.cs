using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemByIdQuery : IRequest<ItemCard>
    {
        public GetItemByIdQuery(string id)
        {
            Id = id;
        }
        
        public string Id { get; set; }
    }

    public class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
    {
        public GetItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .IdMustMatchGuidPattern();
        }
    }
}