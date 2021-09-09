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
        Task UpdateTransactionAsync(UpdateTransactionCommand command, CancellationToken cancellationToken);
    }
}