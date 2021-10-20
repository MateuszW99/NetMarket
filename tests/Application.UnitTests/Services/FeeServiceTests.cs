using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Services;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.UnitTests.Services
{
    public class FeeServiceTests
    {
        public static IEnumerable<object[]> Data =>
            new List<object[]>()
            {
                new object[] { SellerLevel.Beginner, 100m, 10m },
                new object[] { SellerLevel.Intermediate, 100m, 8.5m },
                new object[] { SellerLevel.Advanced, 100m, 6m },
                new object[] { SellerLevel.Business, 100m, 4m },
            };

        private readonly IFeeService sut;

        public FeeServiceTests()
        {
            sut = new FeeService();
        }

        [Theory]
        [InlineData(SellerLevel.Beginner, 0.1)]
        [InlineData(SellerLevel.Intermediate, 0.085)]
        [InlineData(SellerLevel.Advanced, 0.06)]
        [InlineData(SellerLevel.Business, 0.04)]
        public async Task GetFeeRate_Returns_ExistingFeeRate_ForEach_SellerLevel(SellerLevel sellerLevel, decimal expectedRate)
        {
            var result = await sut.GetFeeRate(sellerLevel);
            result.Should().Be(expectedRate);
        }

        [Fact]
        public async Task CalculateFee_Throws_ForNotExistingSellerLevel()
        {
            var notExistingSellerLevel = (SellerLevel)((int)Enum.GetValues<SellerLevel>().Max() + 1);
            var price = 100m;
            await FluentActions.Awaiting(() => sut.CalculateFee(notExistingSellerLevel, price))
                .Should()
                .ThrowAsync<Exception>();
        }

        [Fact]
        public async Task CalculateFee_Should_ReturnAppropriateFee()
        {
            var price = 100m;
            var sellerLevel = SellerLevel.Beginner;
            var expected = 10m;

            var result = await sut.CalculateFee(sellerLevel, price);

            result.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task CalculateFee_Should_ReturnAppropriateFee_ForEachSellerLevel(SellerLevel sellerLevel, decimal price, decimal expected)
        {
            var result = await sut.CalculateFee(sellerLevel, price);

            result.Should().Be(expected);
        }
    }
}