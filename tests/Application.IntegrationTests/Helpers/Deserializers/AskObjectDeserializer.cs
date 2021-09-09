using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Application.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers.Deserializers
{
    public class AskObjectDeserializer : IObjectDeserializer<AskObject>
    {
        public AskObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<AskObject>();
        }

        public List<AskObject> DeserializeToList(HttpContent content)
        {
            throw new System.NotImplementedException();
        }
    }
}