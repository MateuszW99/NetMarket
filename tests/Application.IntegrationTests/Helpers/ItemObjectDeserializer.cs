using System.Net.Http;
using System.Text;
using Application.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers
{
    public class ItemObjectDeserializer : IObjectDeserializer<ItemObject>
    {
        public ItemObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<ItemObject>();
        }
    }
}