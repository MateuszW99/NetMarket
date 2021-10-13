using System;
using System.Collections.Generic;
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
        Task<List<Ask>> GetItemAsks(Guid itemId);
        Task CreateAskAsync(CreateAskCommand command, decimal fee, CancellationToken cancellationToken);
        Task UpdateAskAsync(Ask ask, UpdateAskCommand command, decimal fee, CancellationToken cancellationToken);
        Task DeleteAskAsync(Ask ask, CancellationToken cancellationToken);
    }
}