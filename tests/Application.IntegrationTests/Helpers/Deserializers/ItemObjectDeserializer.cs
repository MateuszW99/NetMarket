using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Application.Models.ApiModels.Items;
using Application.Models.DTOs;
using Newtonsoft.Json.Linq;

namespace Application.IntegrationTests.Helpers.Deserializers
{
    public class ItemObjectDeserializer : IObjectDeserializer<ItemObject>
    {
        public ItemObject Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<ItemObject>();
        }
        
        public List<ItemObject> DeserializeToList(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            var array = (JArray)JObject.Parse(json)["items"];

            var list = new List<ItemObject>();
            foreach (var token in array)
            {
                 list.Add(token.ToObject<ItemObject>());
            }

            return list;
        }
    }

    public class ItemCardDeserializer : IObjectDeserializer<ItemCard>
    {
        public ItemCard Deserialize(HttpContent content)
        {
            var stream = content.ReadAsByteArrayAsync();
            var json = Encoding.UTF8.GetString(stream.Result);
            return JObject.Parse(json).ToObject<ItemCard>();
        }

        public List<ItemCard> DeserializeToList(HttpContent content)
        {
            throw new System.NotImplementedException();
        }
    }
}