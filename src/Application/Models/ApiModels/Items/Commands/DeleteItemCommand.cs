using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Commands
{
    public class DeleteItemCommand : IRequest
    {
        public string Id { get; set; }
    }

    internal class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
    {
        public DeleteItemCommandValidator()
        {
            RuleFor(x => x.Id).IdMustMatchGuidPattern();
        }
    }
}