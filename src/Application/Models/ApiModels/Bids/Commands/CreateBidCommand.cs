using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Commands
{
    public class CreateBidCommand : IRequest
    {
        public string ItemId { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
    }
    
    public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidCommandValidator()
        {
            RuleFor(x => x.ItemId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Size).NotNull().NotEmpty();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}