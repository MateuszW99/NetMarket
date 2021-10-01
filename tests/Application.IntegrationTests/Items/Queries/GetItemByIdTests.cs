using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.IntegrationTests.Helpers.Deserializers;
using Application.Models.ApiModels.Items;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Items.Queries
{
    public class GetItemByIdTests : IntegrationTest
    {
        private readonly IObjectDeserializer<ItemObject> _itemDeserializer;
        private readonly IObjectDeserializer<ItemCard> _itemCardDeserializer;
        
        public GetItemByIdTests()
        {
            _itemDeserializer = new ItemObjectDeserializer();
            _itemCardDeserializer = new ItemCardDeserializer();
        }
        
        [Fact]
        public async Task ShouldItemReturnById()
        {
            ItemObject sampleItem = new()
            {
                Id = "f3f899b0-6571-4f75-9207-f61190e17794",
                Name = "Jordan 4 Retro University Blue",
                ImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=700&h=500&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                SmallImageUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=300&h=214&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                ThumbUrl = "https://images.stockx.com/images/Air-Jordan-4-Retro-University-Blue-Product.jpg?fit=fill&bg=FFFFFF&w=140&h=100&auto=format,compress&trim=color&q=90&dpr=2&updated_at=1616520672",
                Make =  "Jordan 4 Retro",
                Model =  "University Blue",
                Gender = "Men",
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var response = await _client.GetAsync($"{Address.ApiBase}/{Address.Items}/{sampleItem.Id}");

            var itemCard = _itemCardDeserializer.Deserialize(response.Content);
            
            itemCard.Should().NotBeNull(); 
            itemCard.Item.Id.Should().BeEquivalentTo(sampleItem.Id);
            itemCard.Item.Name.Should().BeEquivalentTo(sampleItem.Name);
            itemCard.Item.ImageUrl.Should().BeEquivalentTo(sampleItem.ImageUrl);
            itemCard.Item.SmallImageUrl.Should().BeEquivalentTo(sampleItem.SmallImageUrl);
            itemCard.Item.ThumbUrl.Should().BeEquivalentTo(sampleItem.ThumbUrl);
            itemCard.Item.Make.Should().BeEquivalentTo(sampleItem.Make);
            itemCard.Item.Model.Should().BeEquivalentTo(sampleItem.Model);
            itemCard.Item.Gender.Should().BeEquivalentTo(sampleItem.Gender);
            itemCard.Item.RetailPrice.Should().Be(sampleItem.RetailPrice);
            itemCard.Item.Description.Should().BeEquivalentTo(sampleItem.Description);
        }

        [Fact]
        public async Task ShouldThrowForNotExistingId()
        {
            var query = new GetItemByIdQuery(Guid.NewGuid().ToString());
            await FluentActions.Invoking(() => _mediator.Send(query)).Should().ThrowAsync<NotFoundException>();
        }
    }

}