using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Commands
{
    public class CreateBidCommand : IRequest
    {
        public string ItemId { get; set; }
        public string SizeId { get; set; }
        public string Price { get; set; }
    }
    
    public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidCommandValidator()
        {
            RuleForEach(x => new[] {x.ItemId, x.SizeId}).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}