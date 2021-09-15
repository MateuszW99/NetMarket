using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Commands;
using Domain.Entities;
using Domain.Enums;
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

        public IQueryable<Transaction> GetTransactions(SearchTransactionsQuery query)
        {
            var transactionsQuery = _context.Transactions
                .Include(x => x.Ask).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Status))
            {
                transactionsQuery = transactionsQuery.Where(x => x.Status.ToString() == query.Status);
            }

            return transactionsQuery.OrderBy(x => x.StartDate);
        }

        public IQueryable<Transaction> GetSupervisorTransactions(SearchTransactionsQuery query, string supervisorId)
        {
            var transactionsQuery = _context.Transactions
                .Include(x => x.Ask).DefaultIfEmpty()
                .Include(x => x.Bid).DefaultIfEmpty()
                .Where(x => x.AssignedSupervisorId == Guid.Parse(supervisorId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Status))
            {
                transactionsQuery = transactionsQuery.Where(x => x.Status.ToString() == query.Status);
            }

            return transactionsQuery.OrderBy(x => x.StartDate);
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
            var transaction =
                await _context.Transactions.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Id), cancellationToken);

            if (transaction == null)
            {
                throw new NotFoundException(nameof(Transaction), command.Id);
            }

            transaction.Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true);

            if ((TransactionStatus)Enum.Parse(typeof(TransactionStatus), command.Status, true) ==
                TransactionStatus.Delivered)
            {
                transaction.EndDate = DateTime.Now;
            }

            transaction.SellerFee = command.SellerFee;
            transaction.BuyerFee = command.BuyerFee;
            transaction.Payout = command.Payout;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateTransactionStatusAsync(UpdateTransactionStatusCommand command, string supervisorId,
            CancellationToken cancellationToken)
        {
            var transaction =
                await _context.Transactions.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Id), cancellationToken);

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
    }
}