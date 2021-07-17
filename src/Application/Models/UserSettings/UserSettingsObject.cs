using Application.Common.Mappings;

namespace Application.Models.UserSettings
{
    public class UserSettingsObject : IMapFrom<Domain.Entities.UserSettings>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string SellerLevel { get; set; }
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