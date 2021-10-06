using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Items.Commands;
using Application.Models.DTOs;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly IItemService sut;
        private readonly Mock<IApplicationDbContext> _context;

        public ItemServiceTests()
        {
            _context = new Mock<IApplicationDbContext>();
            sut = new ItemService(_context.Object);
        }

        [Fact]
        public async Task GetItemByIdAsync_Should_ReturnItem_When_ItemWithGivenId_Exists()
        {
            var id = Guid.NewGuid();
            var items = new List<Item>()
            {
                new() { Id = id }
            };

            var mockedItems = items.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Items).Returns(mockedItems.Object);

            var result = await sut.GetItemByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }
        
        [Fact]
        public async Task GetItemByIdAsync_Should_ReturnItem_When_ManyItemsExist()
        {
            var id = Guid.NewGuid();
            var items = new List<Item>()
            {
                new() { Id = id },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() }
            };

            var mockedItems = items.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Items).Returns(mockedItems.Object);

            var result = await sut.GetItemByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }
        
        [Fact]
        public async Task GetItemByIdAsync_Should_ReturnItem_When_ItemWithGivenId_DoesntExist()
        {
            var id = Guid.NewGuid();
            var items = new List<Item>();

            var mockedItems = items.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Items).Returns(mockedItems.Object);

            var result = await sut.GetItemByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetItemsWithCategory_Should_ReturnListOfItems_ForGivenCategory()
        {
            var category = "Sneakers";
            var items = new List<Item>()
            {
                new() { Id = Guid.NewGuid(), Category = category },
                new() { Id = Guid.NewGuid(), Category = category },
                new() { Id = Guid.NewGuid() }
            };

            var mockedItems = items.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Items).Returns(mockedItems.Object);

            var result = await sut.GetItemsWithCategory(category).ToListAsync();
            
            result.ForEach(x => x.Category.Should().Be(category));
            result.Count.Should().Be(2);
        }

        [Theory]
        [InlineData("Sneakers", "Electronics", 3)]
        [InlineData("Collectibles", "", 5)]
        [InlineData("Collectibles", null, 2)]
        public async Task GetItemsWithCategory_Should_ReturnListOfItems_ForGivenCategory_When_ManyItemsWithDifferentCategoriesExist(string categoryToLookup, string otherCategory, int lookupCount)
        {
            var items = new List<Item>()
            {
                new() { Id = Guid.NewGuid(), Category = otherCategory},
                new() { Id = Guid.NewGuid(), Category = otherCategory},
            };
            items.AddRange(Enumerable.Repeat(new Item()
                {
                    Id = Guid.NewGuid(), Category = categoryToLookup
                }, 
                lookupCount));
            
            var mockedItems = items.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Items).Returns(mockedItems.Object);

            var result = await sut.GetItemsWithCategory(categoryToLookup).ToListAsync();
            
            result.ForEach(x => x.Category.Should().Be(categoryToLookup));
            result.Count.Should().Be(lookupCount);
        }
    
        [Fact]
        public async Task CreateItemAsync_Should_CreateItem()
        {
            var brandId = Guid.NewGuid();
            var brand = new Brand()
            {
                Id = brandId, Name = "Nike"
            };
            var price = 200m;
            var category = "Sneakers";
            var description = "Abcd";
            var name = "another nike sneaker";
            var id = Guid.NewGuid();
            var url = "https://stockx.com/something";
            var gender = "Male";
            var make = "Nike";
            var model = "af1";
            var item = new Item()
            {
                Id = id,
                Brand = brand,
                BrandId = brandId,
                Category = category,
                Description = description,
                Name = name,
                RetailPrice = price,
                ImageUrl = url,
                SmallImageUrl = url,
                ThumbUrl = url,
                Gender = gender,
                Model = model,
                Make = make
            };

            var command = new CreateItemCommand()
            {
                Brand = new BrandObject() { Id = brandId.ToString(), Name = "Nile" },
                Category = category,
                Description = description,
                Gender = gender,
                ImageUrl = url,
                SmallImageUrl = url,
                ThumbUrl = url,
                Make = make,
                Model = model,
                Name = name,
                RetailPrice = price
            };

            var itemsBefore = new List<Item>();
            var mockedItemsBefore = itemsBefore.AsQueryable().BuildMockDbSet();
            var brands = new List<Brand>() { brand };
            var mockedBrands = brands.AsQueryable().BuildMockDbSet();
            var itemsAfter = new List<Item>() { item };
            var mockedItemsAfter = itemsAfter.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Brands).Returns(mockedBrands.Object);
            _context.Setup(x => x.Items).Returns(mockedItemsBefore.Object);
            _context.Setup(x => x.Items.AddAsync(It.IsAny<Item>(), CancellationToken.None))
                .Callback(() => _context.Setup(x => x.Items).Returns(mockedItemsAfter.Object));

            var oldCount = _context.Object.Items.CountAsync();
            
            await sut.CreateItemAsync(command, CancellationToken.None);
            var newCount = _context.Object.Items.CountAsync();
            
            var newItem = await _context.Object.Items.FirstOrDefaultAsync(x => x.Id == id);
            newItem.Should().NotBeNull();
            newItem.Should().BeEquivalentTo(item);
            (await newCount).Should().Be(await oldCount + 1);
        }
    }
}