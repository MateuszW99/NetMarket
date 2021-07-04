using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class SizeSeeder : ISeeder<List<Size>>
    {
        public async Task<List<Size>> Seed()
        {
            var jsonData = await File.ReadAllTextAsync("./../../../data/sizes.json");
            var data = JsonConvert.DeserializeObject<string[]>(jsonData);
            return data.Select(x => new Size()
            {
                Id = Guid.NewGuid(),
                Value = x
            }).ToList();
        }
    }
}