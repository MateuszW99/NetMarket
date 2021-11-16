using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Commands;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsByStatus(SearchTransactionsQuery query);
        Task<List<Transaction>> GetSupervisorTransactions(SearchTransactionsQuery query, string supervisorId);
        Task<Transaction> GetTransactionByIdAsync(string transactionId, string supervisorId);
        Task UpdateTransactionAsync(UpdateTransactionCommand command, CancellationToken cancellationToken);
        Task UpdateTransactionStatusAsync(UpdateTransactionStatusCommand command, string supervisorId, CancellationToken cancellationToken);
        Task InitializeTransaction(Ask ask, Bid bid, DateTime startDate, Guid supervisorId, CancellationToken cancellationToken);
        Task<List<Transaction>> GetUserTransactionsAsync(Guid userId, string searchQuery);
    }
}