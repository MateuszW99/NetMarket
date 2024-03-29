﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class BidServiceTests
    {
        private readonly IBidService sut;
        private readonly Mock<IApplicationDbContext> _context;
        public BidServiceTests()
        {
            _context = new Mock<IApplicationDbContext>();
            var httpService = new Mock<IHttpService>();
            sut = new BidService(_context.Object, httpService.Object);
        }
        
        [Fact]
        public async Task GetBidByIdAsync_Should_ReturnBid_When_BidWithGivenId_Exists()
        {
            var id = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = id }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetBidByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetBidByIdAsync_Should_ReturnExactBid_When_ManyBidsExist()
        {
            var id = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = id },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetBidByIdAsync(id);

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetBidByIdAsync_Should_ReturnNull_When_BidWithGivenId_DoesntExist()
        {
            var bids = new List<Bid>();
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            var bidId = Guid.NewGuid();

            var result = await sut.GetBidByIdAsync(bidId);

            result.Should().BeNull();
        }
        
        [Fact]
        public async Task GetBidByIdAsync_Should_ReturnNull_When_ManyBidsExist_But_NoneHasGivenId()
        {
            var id = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetBidByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserBids_Should_ReturnPopulatedListOfUserBids_When_TheyOwnSome()
        {
            var userId = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = Guid.NewGuid(), CreatedBy = userId },
                new() { Id = Guid.NewGuid(), CreatedBy = userId },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetUserBids(userId);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.TrueForAll(x => x.CreatedBy == userId).Should().BeTrue();
        }

        [Fact]
        public async Task GetUserBids_Should_ReturnEmptyList_When_TheyOwnNone()
        {
            var userId = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);

            var result = await sut.GetUserBids(userId);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetItemBids_Should_ReturnPopulatedListOfBids_When_ItemHasBids()
        {
            var itemId = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = Guid.NewGuid(), ItemId = itemId },
                new() { Id = Guid.NewGuid(), ItemId = itemId },
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.Empty }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);

            var result = await sut.GetItemBids(itemId);

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetItemBids_Should_ReturnEmptyListOfBids_When_ItemHasNoBids()
        {
            var itemId = Guid.NewGuid();
            var bids = new List<Bid>()
            {
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), ItemId = Guid.Empty },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);

            var result = await sut.GetItemBids(itemId);

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateBidAsync_Should_CreateNewBid()
        {
            var bidId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var size = new Size { Id = Guid.NewGuid(), Value = "14" };
            var price = 200.00m;
            var fee = 10m;
            
            var bid = new Bid()
            {
                Id = bidId,
                ItemId = itemId,
                SizeId = size.Id,
                Size = size,
                Price = price,
                BuyerFee = fee
            };
            
            var command = new CreateBidCommand()
            {
                ItemId = itemId.ToString(),
                Size = size.Value,
                Price = price.ToString(CultureInfo.InvariantCulture)
            };

            var sizes = new List<Size>() { size };
            var mockedSizes = sizes.AsQueryable().BuildMockDbSet();
            var bidsBefore = new List<Bid>();
            var mockedBidsBefore = bidsBefore.AsQueryable().BuildMockDbSet();
            var bidsAfter = new List<Bid>() { bid };
            var mockedBidsAfter = bidsAfter.AsQueryable().BuildMockDbSet();
            
            _context.Setup(x => x.Sizes).Returns(mockedSizes.Object);
            _context.Setup(x => x.Bids).Returns(mockedBidsBefore.Object);
            _context.Setup(x => x.Bids.AddAsync(It.IsAny<Bid>(), CancellationToken.None))
                .Callback(() => _context.Setup(x => x.Bids).Returns(mockedBidsAfter.Object));
            
            await sut.CreateBidAsync(command, fee, CancellationToken.None);

            var newBid = await mockedBidsAfter.Object.FirstOrDefaultAsync(x => x.Id == bidId);
            newBid.Should().NotBeNull();
            newBid.Id.Should().Be(bidId);
            newBid.SizeId.Should().Be(size.Id);
            newBid.Size.Value.Should().Be(size.Value);
            newBid.ItemId.Should().Be(itemId);
            newBid.Price.Should().Be(price);
            newBid.BuyerFee.Should().Be(fee);
            newBid.UsedInTransaction.Should().Be(false);
        }

        [Fact]
        public async Task UpdateBidAsync_Should_UpdateBid()
        {
            var bidId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var size = new Size { Id = Guid.NewGuid(), Value = "14" };
            var oldPrice = 2.0m;
            var oldBuyerFee = 2.0m;
            var oldBid = new Bid()
            {
                Id = bidId,
                BuyerFee = oldBuyerFee,
                Created = DateTime.UtcNow,
                CreatedBy = userId,
                ItemId = itemId,
                Price = oldPrice,
                LastModifiedBy = null,
                Size = size,
                SizeId = size.Id
            };
            
            var sizes = new List<Size>() { size };
            var mockedSizes = sizes.AsQueryable().BuildMockDbSet();
            
            
            var bidsBefore = new List<Bid>()
            {
                oldBid,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedBidsBefore = bidsBefore.AsQueryable().BuildMockDbSet();
            
            var newPrice = oldPrice + 1m;
            var newFee = oldBuyerFee + 1m;
            var updatedBid = new Bid()
            {
                Id = bidId,
                BuyerFee = newFee,
                CreatedBy = userId,
                ItemId = itemId,
                Price = newPrice,
                SizeId = size.Id,
                Size = size,
                LastModifiedBy = userId
            };
            
            var bidsAfter = new List<Bid>()
            {
                updatedBid,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedBidsAfter = bidsAfter.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Sizes).Returns(mockedSizes.Object);
            _context.Setup(x => x.Bids).Returns(mockedBidsBefore.Object);
            _context.Setup(x => x.Bids.Update(It.IsAny<Bid>()))
                .Callback(() => _context.Setup(x => x.Bids).Returns(mockedBidsAfter.Object));

            var command = new UpdateBidCommand()
            {
                Id = bidId.ToString(),
                Price = newPrice.ToString(CultureInfo.InvariantCulture),
                Size = size.Value
            };
            
            await sut.UpdateBidAsync(oldBid, command, newFee, CancellationToken.None);
            
            var oldCount = await mockedBidsBefore.Object.CountAsync();
            var count = await mockedBidsAfter.Object.CountAsync();
            var updatedBidFromDb = await mockedBidsAfter.Object.FirstOrDefaultAsync(x => x.Id == bidId);
            
            count.Should().Be(oldCount);

            updatedBidFromDb.Should().NotBeNull();
            updatedBidFromDb.SizeId.Should().Be(size.Id);
            updatedBidFromDb.Size.Value.Should().Be(size.Value);
            updatedBidFromDb.ItemId.Should().Be(itemId);
            updatedBidFromDb.Price.Should().Be(newPrice);
            updatedBidFromDb.BuyerFee.Should().Be(newFee);
            updatedBidFromDb.CreatedBy.Should().Be(userId);
            updatedBidFromDb.LastModifiedBy.Should().Be(userId);
            updatedBidFromDb.UsedInTransaction.Should().Be(false);
        }
        
        [Fact]
        public async Task DeleteBidAsync_Should_RemoveBid()
        {
            var bidId = Guid.NewGuid();
            var bid = new Bid() { Id = bidId };
            
            var bidsBefore = new List<Bid>()
            {
                bid,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedBidsBefore = bidsBefore.AsQueryable().BuildMockDbSet();
            
            var bidsAfter  = new List<Bid>()
            {
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedBidsAfter = bidsAfter.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Bids).Returns(mockedBidsBefore.Object);
            _context.Setup(x => x.Bids.Remove(It.IsAny<Bid>()))
                .Callback(() => _context.Setup(x => x.Bids).Returns(mockedBidsAfter.Object));
            
            await sut.DeleteBidAsync(bid, CancellationToken.None);

            var deletedBid = await mockedBidsAfter.Object.FirstOrDefaultAsync(x => x.Id == bidId);
            var oldCount = await mockedBidsBefore.Object.CountAsync();
            var count = await mockedBidsAfter.Object.CountAsync();
            
            deletedBid.Should().BeNull();
            count.Should().Be(oldCount - 1);
        }
    }
}