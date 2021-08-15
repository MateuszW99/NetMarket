using System.Net.Http;
using System.Text;
using Application.Models.DTOs;
using Domain.Enums;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers.Deserializers
{
    public class UserSettingsObjectDeserializer : IObjectDeserializer<UserSettingsObject>
    {
        public UserSettingsObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            
            if (json == string.Empty)
            {
                return new UserSettingsObject()
                {
                    SellerLevel = SellerLevel.Beginner.ToString(),
                    SalesCompleted = 0,
                    PaypalEmail = string.Empty,
                    BillingStreet = string.Empty,
                    BillingAddressLine1 = string.Empty,
                    BillingAddressLine2 = string.Empty,
                    BillingZipCode = string.Empty,
                    BillingCountry = string.Empty,
                    ShippingStreet = string.Empty,
                    ShippingAddressLine1 = string.Empty,
                    ShippingAddressLine2 = string.Empty,
                    ShippingZipCode = string.Empty,
                    ShippingCountry = string.Empty
                };
            }
            
            return JObject.Parse(json).ToObject<UserSettingsObject>();
        }
    }
}