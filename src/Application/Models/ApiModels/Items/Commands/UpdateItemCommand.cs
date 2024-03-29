﻿using Application.Common.Validators;
using Application.Models.DTOs;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Items.Commands
{
    public class UpdateItemCommand : IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Gender { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string ThumbUrl { get; set; }
        public BrandObject Brand { get; set; }
        public string Category { get; set; }
    }

    public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
    {
        public UpdateItemCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .IdMustMatchGuidPattern();
            
            RuleFor(x => x.Make)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(150);

            RuleFor(x => x.Model)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(150);
            
            RuleFor(x => x.Gender)
                .NotNull()
                .MinimumLength(0)
                .MaximumLength(10);
            
            RuleFor(x => x.Description)
                .MinimumLength(0)
                .MaximumLength(1500);

            RuleFor(x => x.RetailPrice)
                .GreaterThanOrEqualTo((decimal) 0.0);
            
            RuleFor(x => x.ImageUrl).MustMatchUrlPattern();
            
            RuleFor(x => x.SmallImageUrl).MustMatchUrlPattern();

            RuleFor(x => x.ThumbUrl).MustMatchUrlPattern();

            RuleFor(x => x.Brand).NotNull();
            
            RuleFor(x => x.Category).NotNull()
                .NotEmpty()
                .MaximumLength(150);
        }
    }
}