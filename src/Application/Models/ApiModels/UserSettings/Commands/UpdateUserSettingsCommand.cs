using Application.Common.Validators;
using FluentValidation;
using MediatR;

namespace Application.Models.ApiModels.UserSettings.Commands
{
    public class UpdateUserSettingsCommand : IRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PaypalEmail { get; set; }

        public string BillingStreet { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingCountry { get; set; }

        public string ShippingStreet { get; set; }
        public string ShippingAddressLine1 { get; set; }
        public string ShippingAddressLine2 { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingCountry { get; set; }
    }

    public class UpdateUserSettingsCommandValidator : AbstractValidator<UpdateUserSettingsCommand>
    {
        public UpdateUserSettingsCommandValidator()
        
        {
            RuleFor(x => x.FirstName).MaximumLength(50);
            RuleFor(x => x.LastName).MaximumLength(50);

            RuleFor(x => x.PaypalEmail).EmailAddress().MaximumLength(40);

            RuleFor(x => x.BillingStreet).MaximumLength(50);
            RuleFor(x => x.BillingAddressLine1).MaximumLength(50);
            RuleFor(x => x.BillingAddressLine2).MaximumLength(50);
            RuleFor(x => x.BillingZipCode).MustMatchZipCodePattern().MaximumLength(6);
            RuleFor(x => x.BillingCountry).MaximumLength(50);

            RuleFor(x => x.ShippingStreet).MaximumLength(50);
            RuleFor(x => x.ShippingAddressLine1).MaximumLength(50);
            RuleFor(x => x.ShippingAddressLine2).MaximumLength(50);
            RuleFor(x => x.ShippingZipCode).MustMatchZipCodePattern().MaximumLength(6);
            RuleFor(x => x.ShippingCountry).MaximumLength(50);
        }
    }
}