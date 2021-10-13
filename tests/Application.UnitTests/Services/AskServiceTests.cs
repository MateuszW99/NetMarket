using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class AskServiceTests
    {
        private readonly IAskService sut;
        private readonly Mock<IApplicationDbContext> _context;

        public AskServiceTests()
        {
            _context = new Mock<IApplicationDbContext>();
            sut = new AskService(_context.Object);
        }
        
        [Fact]
        public async Task GetAskByIdAsync_Should_ReturnAsk_When_AskWithGivenId_Exists()
        {
            var id = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = id }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            
            var result = await sut.GetAskByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAskByIdAsync_Should_ReturnExactAsk_When_ManyAsksExist()
        {
            var id = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = id },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            
            var result = await sut.GetAskByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetAskByIdAsync_Should_ReturnNull_When_AskWithGivenId_DoesntExist()
        {
            var asks = new List<Ask>();
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            var askId = Guid.NewGuid();

            var result = await sut.GetAskByIdAsync(askId);

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task GetAskByIdAsync_Should_ReturnNull_When_ManyAsksExist_But_NoneHasGivenId()
        {
            var id = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            
            var result = await sut.GetAskByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserAsks_Should_ReturnPopulatedListOfUserAsks_When_TheyOwnSome()
        {
            var userId = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = Guid.NewGuid(), CreatedBy = userId },
                new() { Id = Guid.NewGuid(), CreatedBy = userId },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            
            var result = await sut.GetUserAsks(userId).ToListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.TrueForAll(x => x.CreatedBy == userId).Should().BeTrue();
        }

        [Fact]
        public async Task GetUserAsks_Should_ReturnEmptyList_When_TheyOwnNone()
        {
            var userId = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);
            
            var result = await sut.GetUserAsks(userId).ToListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetItemAsks_Should_ReturnPopulatedListOfAsks_When_ItemHasAsks()
        {
            var itemId = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = Guid.NewGuid(), ItemId = itemId },
                new() { Id = Guid.NewGuid(), ItemId = itemId },
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.Empty }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);

            var result = await sut.GetItemAsks(itemId);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetItemAsks_Should_ReturnEmptyListOfAsks_When_ItemHasNoAsks()
        {
            var itemId = Guid.NewGuid();
            var asks = new List<Ask>()
            {
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.Empty },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedAsks = asks.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Asks).Returns(mockedAsks.Object);

            var result = await sut.GetItemAsks(itemId);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateAskAsync_Should_CreateNewAsk()
        {
            var askId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var sizeId = Guid.NewGuid();
            var price = 200.00m;
            var fee = 10m;
            
            var ask = new Ask()
            {
                Id = askId,
                ItemId = itemId,
                SizeId = sizeId,
                Price = price,
                SellerFee = fee
            };
            
            var command = new CreateAskCommand()
            {
                ItemId = itemId.ToString(),
                SizeId = sizeId.ToString(),
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            
            var asksBefore = new List<Ask>();
            var mockedAsksBefore = asksBefore.AsQueryable().BuildMockDbSet();
            var asksAfter = new List<Ask>() { ask };
            var mockedAsksAfter = asksAfter.AsQueryable().BuildMockDbSet();
            
            _context.Setup(x => x.Asks).Returns(mockedAsksBefore.Object);
            _context.Setup(x => x.Asks.AddAsync(It.IsAny<Ask>(), CancellationToken.None))
                .Callback(() => _context.Setup(x => x.Asks).Returns(mockedAsksAfter.Object));
            
            await sut.CreateAskAsync(command, fee, CancellationToken.None);

            var newAsk = await mockedAsksAfter.Object.FirstOrDefaultAsync(x => x.Id == askId);
            newAsk.Should().NotBeNull();
            newAsk.Id.Should().Be(askId);
            newAsk.SizeId.Should().Be(sizeId);
            newAsk.ItemId.Should().Be(itemId);
            newAsk.Price.Should().Be(price);
            newAsk.SellerFee.Should().Be(fee);
        }
        
        [Fact]
        public async Task UpdateAskAsync_Should_UpdateAsk()
        {
            var askId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var sizeId = Guid.NewGuid();
            var oldPrice = 2.0m;
            var oldBuyerFee = 2.0m;
            var oldAsk = new Ask()
            {
                Id = askId,
                SellerFee = oldBuyerFee,
                Created = DateTime.UtcNow,
                CreatedBy = userId,
                ItemId = itemId,
                Price = oldPrice,
                LastModifiedBy = null,
                SizeId = sizeId
            };
            
            var asksBefore = new List<Ask>()
            {
                oldAsk,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedAsksBefore = asksBefore.AsQueryable().BuildMockDbSet();

            var newPrice = oldPrice + 1m;
            var newFee = oldBuyerFee + 1m;
            var updatedAsk = new Ask()
            {
                Id = askId,
                SellerFee = newFee,
                CreatedBy = userId,
                ItemId = itemId,
                Price = newPrice,
                SizeId = sizeId,
                LastModifiedBy = userId
            };
            
            var asksAfter = new List<Ask>()
            {
                updatedAsk,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedAsksAfter = asksAfter.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Asks).Returns(mockedAsksBefore.Object);
            _context.Setup(x => x.Asks.Update(It.IsAny<Ask>()))
                .Callback(() => _context.Setup(x => x.Asks).Returns(mockedAsksAfter.Object));

            var command = new UpdateAskCommand()
            {
                Id = askId.ToString(),
                Price = newPrice.ToString(CultureInfo.InvariantCulture),
                SizeId = sizeId.ToString()
            };
            
            await sut.UpdateAskAsync(oldAsk, command, newFee, CancellationToken.None);
            
            var oldCount = await mockedAsksBefore.Object.CountAsync();
            var count = await mockedAsksAfter.Object.CountAsync();
            var updatedAskFromDb = await mockedAsksAfter.Object.FirstOrDefaultAsync(x => x.Id == askId);
            
            count.Should().Be(oldCount);

            updatedAskFromDb.Should().NotBeNull();
            updatedAskFromDb.SizeId.Should().Be(sizeId);
            updatedAskFromDb.ItemId.Should().Be(itemId);
            updatedAskFromDb.Price.Should().Be(newPrice);
            updatedAskFromDb.SellerFee.Should().Be(newFee);
            updatedAskFromDb.CreatedBy.Should().Be(userId);
            updatedAskFromDb.LastModifiedBy.Should().Be(userId);
        }

        [Fact]
        public async Task DeleteAskAsync_Should_RemoveAsk()
        {
            var askId = Guid.NewGuid();
            var ask = new Ask() { Id = askId };
            
            var asksBefore = new List<Ask>()
            {
                ask,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedAsksBefore = asksBefore.AsQueryable().BuildMockDbSet();
            
            var asksAfter  = new List<Ask>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedAsksAfter = asksAfter.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Asks).Returns(mockedAsksBefore.Object);
            _context.Setup(x => x.Asks.Remove(It.IsAny<Ask>()))
                .Callback(() => _context.Setup(x => x.Asks).Returns(mockedAsksAfter.Object));
            
            await sut.DeleteAskAsync(ask, CancellationToken.None);

            var deletedAsk = await mockedAsksAfter.Object.FirstOrDefaultAsync(x => x.Id == askId);
            var oldCount = await mockedAsksBefore.Object.CountAsync();
            var count = await mockedAsksAfter.Object.CountAsync();
            
            deletedAsk.Should().BeNull();
            count.Should().Be(oldCount - 1);
        }
    }
}