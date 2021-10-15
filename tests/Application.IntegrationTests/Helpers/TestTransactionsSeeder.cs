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
                await context.Transactions.AddRangeAsync(GetTransactions(factory.CurrentUserId));
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        private static IEnumerable<Transaction> GetTransactions(string supervisorId)
        {
            var bidId = Guid.NewGuid();
            var askId = Guid.NewGuid();
            return new List<Transaction>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AssignedSupervisorId = Guid.NewGuid(),
                    // AskId = askId,
                    // Ask = new Ask() { Id = askId, Price = 148M, SellerFee = 8M },
                    // BidId = bidId,
                    // Bid = new Bid() { Id = bidId, Price = 163M, BuyerFee = 15M },
                    Ask = new Ask() { Id = Guid.NewGuid() },
                    Bid = new Bid() { Id = Guid.NewGuid() },
                    Status = TransactionStatus.Delivered,
                    StartDate =  DateTime.ParseExact("08/20/2021", "MM/dd/yyyy", null),
                    EndDate =  DateTime.ParseExact("08/25/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    TotalBuyerCost = 163M, 
                    BuyerFee = 15M,
                    SellerPayout = 140M,
                    CompanyProfit = 23M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AssignedSupervisorId = Guid.NewGuid(),
                    Ask = new Ask() { Id = Guid.NewGuid() },
                    Bid = new Bid() { Id = Guid.NewGuid() },
                    Status = TransactionStatus.Started,
                    StartDate =  DateTime.ParseExact("08/24/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    TotalBuyerCost = 163M, 
                    BuyerFee = 15M,
                    SellerPayout = 140M,
                    CompanyProfit = 23M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AssignedSupervisorId = Guid.Parse(supervisorId),
                    Ask = new Ask() { Id = Guid.NewGuid() },
                    Bid = new Bid() { Id = Guid.NewGuid() },
                    Status = TransactionStatus.Started,
                    StartDate =  DateTime.ParseExact("08/25/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    TotalBuyerCost = 163M, 
                    BuyerFee = 15M,
                    SellerPayout = 140M,
                    CompanyProfit = 23M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AssignedSupervisorId = Guid.Parse(supervisorId),
                    Ask = new Ask() { Id = Guid.NewGuid() },
                    Bid = new Bid() { Id = Guid.NewGuid() },
                    Status = TransactionStatus.Checked,
                    StartDate =  DateTime.ParseExact("08/22/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    TotalBuyerCost = 163M, 
                    BuyerFee = 15M,
                    SellerPayout = 140M,
                    CompanyProfit = 23M
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AssignedSupervisorId = Guid.Parse(supervisorId),
                    Ask = new Ask() { Id = Guid.NewGuid() },
                    Bid = new Bid() { Id = Guid.NewGuid() },
                    Status = TransactionStatus.EnRouteFromWarehouse,
                    StartDate =  DateTime.ParseExact("08/18/2021", "MM/dd/yyyy", null),
                    SellerFee = 8M,
                    TotalBuyerCost = 163M, 
                    BuyerFee = 15M,
                    SellerPayout = 140M,
                    CompanyProfit = 23M
                },
            };
        }
    }
}