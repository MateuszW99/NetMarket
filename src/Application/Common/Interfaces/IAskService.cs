using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.ApiModels.Asks.Commands;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IAskService
    {
        Task<Ask> GetAskByIdAsync(Guid askId);
        IQueryable<Ask> GetUserAsks(Guid userId);
        IQueryable<Ask> GetItemAsks(Guid id);
        Task CreateAskAsync(CreateAskCommand command, decimal fee, CancellationToken cancellationToken);
        Task UpdateAskAsync(Ask ask, UpdateAskCommand command, decimal fee, CancellationToken cancellationToken);
        Task DeleteAskAsync(Ask ask, CancellationToken cancellationToken);
    }
}