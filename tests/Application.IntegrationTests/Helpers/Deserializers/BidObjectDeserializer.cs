using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Application.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers.Deserializers
{
    public class BidObjectDeserializer : IObjectDeserializer<BidObject>
    {
        public BidObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<BidObject>();
        }

        public List<BidObject> DeserializeToList(HttpContent content)
        {
            throw new System.NotImplementedException();
        }
    }
}