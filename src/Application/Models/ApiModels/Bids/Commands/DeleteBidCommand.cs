using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Commands
{
    public class DeleteBidCommand : IRequest
    {
        public string Id { get; set; }
    }
    
    public class DeleteBidCommandValidator : AbstractValidator<DeleteBidCommand>
    {
        public DeleteBidCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
        }
    }
}