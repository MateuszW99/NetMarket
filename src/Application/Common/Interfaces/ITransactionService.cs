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
        IQueryable<Transaction> GetTransactions(SearchTransactionsQuery query);
        IQueryable<Transaction> GetSupervisorTransactions(SearchTransactionsQuery query, string supervisorId);
        Task<Transaction> GetTransactionByIdAsync(string transactionId, string supervisorId);
        Task UpdateTransactionAsync(UpdateTransactionCommand command, CancellationToken cancellationToken);
        Task UpdateTransactionStatusAsync(UpdateTransactionStatusCommand command, string supervisorId, CancellationToken cancellationToken);
        Task BuyNow();
        Task SellNow();
    }
}