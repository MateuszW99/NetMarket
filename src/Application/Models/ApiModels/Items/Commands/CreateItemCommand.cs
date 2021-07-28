using System;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Commands
{
    public class CreateItemCommand : IRequest
    {
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public string Brand { get; set; }
    }

    internal class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        public CreateItemCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(150);

            RuleFor(x => x.Make)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(150);

            RuleFor(x => x.Model)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(150);
            
            RuleFor(x => x.Description)
                .MinimumLength(0)
                .MaximumLength(1500);

            RuleFor(x => x.RetailPrice)
                .GreaterThanOrEqualTo((decimal) 0.0);

            RuleFor(x => x.ImageUrl)
                .NotNull()
                .NotEmpty()
                .When(x => Uri.IsWellFormedUriString(x.ImageUrl, UriKind.Absolute));
            
            RuleFor(x => x.SmallImageUrl)
                .NotNull()
                .NotEmpty()
                .When(x => Uri.IsWellFormedUriString(x.SmallImageUrl, UriKind.Absolute));
            
            RuleFor(x => x.ThumbUrl)
                .NotNull()
                .NotEmpty()
                .When(x => Uri.IsWellFormedUriString(x.ThumbUrl, UriKind.Absolute));
            
            RuleFor(x => x.Brand)
                .NotNull()
                .NotEmpty();
        }
    }
}