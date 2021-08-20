using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Application.IntegrationTests.Items.Queries
{
    public class GetItemsWithCategoryTests : IntegrationTest
    {
        private readonly IObjectDeserializer<ItemObject> _itemDeserializer;

        public GetItemsWithCategoryTests()
        {
            _itemDeserializer = new ItemObjectDeserializer();
        }

        [Fact]
        public async Task ShouldReturnNotEmptyList()
        {
            var query = new ItemsWithCategoryQuery
            {
                Category = "Sneakers"
            };

            var response = await _client.GetAsync($"{Address.ApiBase}/{Address.Items}/{Address.Category}?category={query.Category}");
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            
            var items = _itemDeserializer.DeserializeToList(response.Content);

            items.Should().NotBeNullOrEmpty();
            items.ForEach(x => x.Category.Should().Be("Sneakers"));
        }
        
        [Fact]
        public async Task ShouldReturnEmptyList()
        {
            var query = new ItemsWithCategoryQuery
            {
                Category = "randomString"
            };

            var response = await _client.GetAsync($"{Address.ApiBase}/{Address.Items}/{Address.Category}?category={query.Category}");
            
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            
            var items = _itemDeserializer.DeserializeToList(response.Content);
            items.Should().BeEmpty();
        }
    }
}