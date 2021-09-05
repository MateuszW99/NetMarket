using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests.Helpers
{
    public static class TestTransactionsSeeder
    {
        public static async Task SeedTestTransactionsAsync(CustomWebApplicationFactory factory)
        {
            var context = DbHelper.GetDbContext(factory);

            if (!await context.Transactions.AnyAsync())
            {
                await context.Transactions.AddRangeAsync(GetTransactions());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        private static IEnumerable<Transaction> GetTransactions()
        {
            return new List<Transaction>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AskId = Guid.NewGuid(),
                    BidId = Guid.NewGuid(),
                    Status = TransactionStatus.Delivered,
                    StartDate =  DateTime.ParseExact("08/20/2021", "MM/dd/yyyy", null),
                    EndDate =  DateTime.ParseExact("08/25/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    BuyerFee = 150M,
                    Payout = 140M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AskId = Guid.NewGuid(),
                    BidId = Guid.NewGuid(),
                    Status = TransactionStatus.Started,
                    StartDate =  DateTime.ParseExact("08/24/2021", "MM/dd/yyyy", null),
                    SellerFee = 7M,
                    BuyerFee = 160M,
                    Payout = 150M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AskId = Guid.NewGuid(),
                    BidId = Guid.NewGuid(),
                    Status = TransactionStatus.Started,
                    StartDate =  DateTime.ParseExact("08/25/2021", "MM/dd/yyyy", null),
                    SellerFee = 10M,
                    BuyerFee = 180M,
                    Payout = 165M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AskId = Guid.NewGuid(),
                    BidId = Guid.NewGuid(),
                    Status = TransactionStatus.Checked,
                    StartDate =  DateTime.ParseExact("08/22/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    BuyerFee = 150M,
                    Payout = 140M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AskId = Guid.NewGuid(),
                    BidId = Guid.NewGuid(),
                    Status = TransactionStatus.EnRouteFromWarehouse,
                    StartDate =  DateTime.ParseExact("08/18/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    BuyerFee = 150M,
                    Payout = 140M
                },
            };
        }
    }
}