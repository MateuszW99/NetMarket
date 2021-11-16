using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Commands;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IApplicationDbContext _context;

        public TransactionService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByStatus(SearchTransactionsQuery query)
        {
            var transactionsQuery = _context.Transactions
                .Include(x => x.Ask).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Status))
            {
                transactionsQuery = transactionsQuery.Where(x => x.Status.ToString() == query.Status);
            }

            return await transactionsQuery.OrderBy(x => x.StartDate).ToListAsync();
        }

        public async Task<List<Transaction>> GetSupervisorTransactions(SearchTransactionsQuery query, string supervisorId)
        {
            var transactionsQuery = _context.Transactions
                .Include(x => x.Ask)
                .ThenInclude(y => y.Item).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .Where(x => x.AssignedSupervisorId == Guid.Parse(supervisorId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Status))
            {
                transactionsQuery = transactionsQuery.Where(x => x.Status == (TransactionStatus)Enum.Parse(typeof(TransactionStatus), query.Status));
            }

            return await transactionsQuery.OrderBy(x => x.StartDate).ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(string transactionId, string supervisorId)
        {
            var transaction = await _context.Transactions
                .Include(x => x.Ask).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .FirstOrDefaultAsync(x =>
                    x.Id == Guid.Parse(transactionId));

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), transactionId);
            }

            if (transaction.AssignedSupervisorId != Guid.Parse(supervisorId))
            {
                throw new ForbiddenAccessException();
            }

            return transaction;
        }

        public async Task UpdateTransactionAsync(UpdateTransactionCommand command, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(
                x => x.Id == Guid.Parse(command.Id),
                cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), command.Id);
            }

            transaction.Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true);

            if ((TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true) == TransactionStatus.Delivered)
            {
                transaction.EndDate = DateTime.Now;
            }

            // TODO: could IFeeService do this?
            transaction.SellerFee = command.SellerFee;
            transaction.BuyerFee = command.BuyerFee;
            transaction.SellerPayout = command.Payout;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTransactionStatusAsync(UpdateTransactionStatusCommand command, string supervisorId, CancellationToken cancellationToken)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(
                x => x.Id == Guid.Parse(command.Id),
                cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), command.Id);
            }

            if (transaction.AssignedSupervisorId != Guid.Parse(supervisorId))
            {
                throw new ForbiddenAccessException();
            }

            transaction.Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true);

            if ((TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true) ==
                TransactionStatus.Delivered)
            {
                transaction.EndDate = DateTime.Now;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task InitializeTransaction(Ask ask, Bid bid, DateTime startDate, Guid supervisorId, CancellationToken cancellationToken)
        {
            var transaction = new Transaction()
            {
                AssignedSupervisorId = supervisorId,
                CompanyProfit = ask.SellerFee + bid.BuyerFee,
                Ask = ask,
                AskId = ask.Id,
                SellerFee = ask.SellerFee,
                SellerPayout = ask.Price - ask.SellerFee,
                Bid = bid,
                BidId = bid.Id,
                TotalBuyerCost = bid.Price + bid.BuyerFee,
                BuyerFee = bid.BuyerFee,
                StartDate = startDate,
                Status = TransactionStatus.Started,
                DomainEvents = new List<DomainEvent> { new TransactionInitializedEvent(ask.CreatedBy) }
            };

            ask.UsedInTransaction = true;
            bid.UsedInTransaction = true;
            
            await _context.Transactions.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Transaction>> GetUserTransactionsAsync(Guid userId, string searchQuery)
        {
            var transactionsQuery = _context.Transactions
                .Include(x => x.Ask)
                .ThenInclude(y => y.Item).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .Where(x => x.Ask.CreatedBy == userId || x.Bid.CreatedBy == userId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                transactionsQuery = transactionsQuery.Where(x => x.Ask.Item.Name.ToLower().Contains(searchQuery.ToLower()));
            }

            return await transactionsQuery.OrderBy(x => x.StartDate).ToListAsync();
        }
    }
}