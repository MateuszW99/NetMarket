using Application.Common.Validators;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Asks.Queries
{
    public class GetAskByIdQuery : IRequest<AskObject>
    {
        public string Id { get; set; }
    }

    public class GetAskByIdQueryValidator : AbstractValidator<GetAskByIdQuery>
    {
        public GetAskByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
        }
    }
}