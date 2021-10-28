using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class UserSettings : BaseEntity, IHasDomainEvent
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
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
        
        public List<DomainEvent> DomainEvents { get; set; }
    }
}