﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Items.Queries
{
    public class GetItemByIdTests : IntegrationTest
    {
        private readonly IObjectDeserializer<ItemObject> _itemDeserializer;
        
        public GetItemByIdTests()
        {
            _itemDeserializer = new ItemObjectDeserializer();
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
                RetailPrice = 200,
                Description = "Jordan Brand paid homage to MJ’s alma mater with the Air Jordan 4 University Blue. The University Blue colorway draws a close resemblance to the extremely rare Air Jordan 4 UNC PE that was given to Tarheel student-athletes in 2019.\name\name\nThe Air Jordan 4 University Blue features a University Blue suede upper with mesh netting on the quarter panel and tongue. Similar to OG Jordan 4 colorways of the past, the eyelets, heel tab, and sections of the midsole are a speckled Cement Grey. Hits of black appear on the wing eyelets, sole, and Jumpman logo on the heel tab. Two woven labels are stitched to the tongue; one being the classic Jumpman woven label; the other being a Team Jordan jock tag. A black, white, and Tech Grey sole with a clear Air unit completes the design.\name\name\nThe Air Jordan 4 University Blue released in April of 2021 and retailed for $200."
            };
            
            var response = await _client.GetAsync($"{ApiBaseAddress}/{ItemsAddress}/{sampleItem.Id}");
            
            var item = _itemDeserializer.Deserialize(response.Content);
            
            item.Should().NotBeNull();
            item.Id.Should().BeEquivalentTo(sampleItem.Id);
            item.Name.Should().BeEquivalentTo(sampleItem.Name);
            item.ImageUrl.Should().BeEquivalentTo(sampleItem.ImageUrl);
            item.SmallImageUrl.Should().BeEquivalentTo(sampleItem.SmallImageUrl);
            item.ThumbUrl.Should().BeEquivalentTo(sampleItem.ThumbUrl);
            item.Make.Should().BeEquivalentTo(sampleItem.Make);
            item.Model.Should().BeEquivalentTo(sampleItem.Model);
            item.RetailPrice.Should().Be(sampleItem.RetailPrice);
            item.Description.Should().BeEquivalentTo(sampleItem.Description);
        }

        [Fact]
        public async Task ShouldThrowForNotExistingId()
        {
            var query = new GetItemByIdQuery(Guid.NewGuid().ToString());
            await FluentActions.Invoking(() => _mediator.Send(query)).Should().ThrowAsync<NotFoundException>();
        }
    }

}