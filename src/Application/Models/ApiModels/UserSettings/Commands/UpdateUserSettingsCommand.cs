﻿using Domain.Enums;
using MediatR;

namespace Application.Models.ApiModels.UserSettings.Commands
{
    public class UpdateUserSettingsCommand : IRequest
    {
        public SellerLevel SellerLevel { get; set; }
        public int SalesCompleted { get; set; }
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
}