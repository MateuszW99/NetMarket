using System;
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
            sut = new BidService(_context.Object);
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
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() }
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
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() }
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
                new() {Id = Guid.NewGuid(), CreatedBy = userId },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetUserBids(userId).ToListAsync();

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
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() },
                new() {Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);
            
            var result = await sut.GetUserBids(userId).ToListAsync();

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
                new() {Id = Guid.NewGuid(), ItemId = Guid.Empty }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);

            var result = await sut.GetItemBids(itemId).ToListAsync();

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
                new() {Id = Guid.NewGuid(), ItemId = Guid.Empty },
                new() { Id = Guid.NewGuid() }
            };
            
            var mockedBids = bids.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Bids).Returns(mockedBids.Object);

            var result = await sut.GetItemBids(itemId).ToListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }

        [Fact]
        public async Task CreateBidAsync_Should_CreateNewBid()
        {
            var bidId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var sizeId = Guid.NewGuid();
            var price = 200.00m;
            var fee = 10m;
            
            var bid = new Bid()
            {
                Id = bidId,
                ItemId = itemId,
                SizeId = sizeId,
                Price = price,
                BuyerFee = fee
            };
            
            var command = new CreateBidCommand()
            {
                ItemId = itemId.ToString(),
                SizeId = sizeId.ToString(),
                Price = price.ToString(CultureInfo.InvariantCulture)
            };
            
            var bidsBefore = new List<Bid>();
            var mockedBidsBefore = bidsBefore.AsQueryable().BuildMockDbSet();
            var bidsAfter = new List<Bid>() { bid };
            var mockedBidsAfter = bidsAfter.AsQueryable().BuildMockDbSet();
            
            _context.Setup(x => x.Bids).Returns(mockedBidsBefore.Object);
            _context.Setup(x => x.Bids.AddAsync(It.IsAny<Bid>(), CancellationToken.None))
                .Callback(() => _context.Setup(x => x.Bids).Returns(mockedBidsAfter.Object));
            
            await sut.CreateBidAsync(command, fee, CancellationToken.None);

            var newBid = await mockedBidsAfter.Object.FirstOrDefaultAsync(x => x.Id == bidId);
            newBid.Should().NotBeNull();
            newBid.Id.Should().Be(bidId);
            newBid.SizeId.Should().Be(sizeId);
            newBid.ItemId.Should().Be(itemId);
            newBid.Price.Should().Be(price);
            newBid.BuyerFee.Should().Be(fee);
        }
        
        // TODO: add update-Bid tests

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