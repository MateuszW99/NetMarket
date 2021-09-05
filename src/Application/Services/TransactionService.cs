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
    }
}