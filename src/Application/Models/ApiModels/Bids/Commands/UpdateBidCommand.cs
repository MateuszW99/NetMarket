using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Bids.Commands
{
    public class UpdateBidCommand : IRequest
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string SizeId { get; set; }
        public string Price { get; set; }
    }
    
    public class UpdateBidCommandValidator : AbstractValidator<UpdateBidCommand>
    {
        public UpdateBidCommandValidator()
        {
            RuleForEach(x => new[] {x.Id, x.ItemId, x.SizeId}).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}