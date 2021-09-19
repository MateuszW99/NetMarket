using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Application.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers.Deserializers
{
    public class TransactionObjectDeserializer : IObjectDeserializer<TransactionObject>
    {
        public TransactionObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<TransactionObject>();
        }

        public List<TransactionObject> DeserializeToList(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            var array = (JArray)JObject.Parse(json)["items"];

            var list = new List<TransactionObject>();
            foreach (var token in array)
            {
                list.Add(token.ToObject<TransactionObject>());
            }

            return list;
        }
    }
}