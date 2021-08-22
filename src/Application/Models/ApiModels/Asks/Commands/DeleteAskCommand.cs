using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Asks.Commands
{
    public class DeleteAskCommand : IRequest
    {
        public string Id { get; set; }
    }

    public class DeleteAskCommandValidator : AbstractValidator<DeleteAskCommand>
    {
        public DeleteAskCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
        }
    }
}