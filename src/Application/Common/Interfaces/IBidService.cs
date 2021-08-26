using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.ApiModels.Bids.Commands;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IBidService
    {
        Task<Bid> GetBidByIdAsync(Guid bidId);
        IQueryable<Bid> GetUserBids(Guid userId);
        IQueryable<Bid> GetItemBids(Guid id);
        Task CreateBidAsync(CreateBidCommand command, CancellationToken cancellationToken);
        Task UpdateBidAsync(Bid bid, UpdateBidCommand command, Guid userId, CancellationToken cancellationToken);
        Task DeleteBidAsync(Bid bid, CancellationToken cancellationToken);
    }
}