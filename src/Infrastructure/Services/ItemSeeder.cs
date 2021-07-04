using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Decimal;

namespace Infrastructure.Services
{
    public class ItemSeeder : ISeeder<List<Item>>
    {
        public async Task<List<Item>> Seed()
        {
            var jsonData = await File.ReadAllTextAsync("Data/products.json");
            var data = JsonConvert.DeserializeObject<List<Item>>(jsonData, new ItemConverter());
            return data;
        }
    }

    internal sealed class ItemConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            DateTime.TryParse(obj.SelectToken("releaseDate").ToString(), out var releaseDate);
            
            Item item = new Item()
            {
                Id = new Guid(obj.SelectToken("id").ToString()),
                Name = obj.SelectToken("name").ToString(),
                RetailPrice = Parse(obj.SelectToken("retailPrice").ToString()),
                ImageUrl = obj.SelectToken("media").SelectToken("imageUrl").ToString(),
                SmallImageUrl = obj.SelectToken("media").SelectToken("smallImageUrl").ToString(),
                ThumbUrl = obj.SelectToken("media").SelectToken("thumbUrl").ToString(),
                Brand = new Brand()
                {
                    Id = Guid.NewGuid(),
                    Name = obj.SelectToken("brand").ToString()
                },
                ReleaseDate = releaseDate,
                Model = obj.SelectToken("model").ToString(),
                Make = obj.SelectToken("make").ToString(),
                Description = obj.SelectToken("description").ToString()
            };
            return item;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Item);
        }
    }
}