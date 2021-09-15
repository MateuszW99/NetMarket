﻿using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.Asks.Commands
{
    public class UpdateAskCommand : IRequest
    {
        public string Id { get; set; }
        public string SizeId { get; set; }
        public string Price { get; set; }
    }
    
    public class UpdateAskCommandValidator : AbstractValidator<UpdateAskCommand>
    {
        public UpdateAskCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.SizeId).NotNull().IdMustMatchGuidPattern();
            RuleFor(x => x.Price).NotNull().NotEmpty();
            RuleFor(x => decimal.Parse(x.Price)).GreaterThan((decimal) 0.0);
        }
    }
}