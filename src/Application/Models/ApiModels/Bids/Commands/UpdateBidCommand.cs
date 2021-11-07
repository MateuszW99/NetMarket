using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Commands
{
    public class UpdateBidCommand : IRequest
    {
        public string Id { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
    }
    
    public class UpdateBidCommandValidator : AbstractValidator<UpdateBidCommand>
    {
        public UpdateBidCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Size).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}