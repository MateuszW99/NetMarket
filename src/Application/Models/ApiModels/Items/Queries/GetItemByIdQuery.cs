using Application.Common.Validators;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemByIdQuery : IRequest<ItemObject>
    {
        public GetItemByIdQuery(string id)
        {
            Id = id;
        }
        
        public string Id { get; set; }
    }

    internal class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
    {
        public GetItemByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .IdMustMatchGuidPattern();
        }
    }
}