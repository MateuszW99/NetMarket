using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public async Task<List<Item>> SeedAsync()
        {
            var jsonData = await File.ReadAllTextAsync("Data/products.json");
            var data = JsonConvert.DeserializeObject<List<Item>>(jsonData, new ItemConverter());
            return data;
        }
    }

    internal sealed class ItemConverter : JsonConverter
    {
        private static List<Brand> _brands { get; set; }

        static ItemConverter()
        {
            _brands = new();
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var brandName = obj.SelectToken("brand").ToString();
            var brand = _brands.FirstOrDefault(x => x.Name == brandName);
            if (brand == null)
            {
                brand = new Brand()
                {
                    Name = brandName,
                    Id = Guid.NewGuid()
                };
                _brands.Add(brand);
            }    
                 

            DateTime.TryParse(obj.SelectToken("releaseDate").ToString(), out var releaseDate);
            
            Item item = new Item()
            {
                Id = new Guid(obj.SelectToken("id").ToString()),
                Name = obj.SelectToken("name").ToString(),
                RetailPrice = Parse(obj.SelectToken("retailPrice").ToString()),
                ImageUrl = obj.SelectToken("media").SelectToken("imageUrl").ToString(),
                SmallImageUrl = obj.SelectToken("media").SelectToken("smallImageUrl").ToString(),
                ThumbUrl = obj.SelectToken("media").SelectToken("thumbUrl").ToString(),
                BrandId = brand.Id,
                Brand = brand,
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