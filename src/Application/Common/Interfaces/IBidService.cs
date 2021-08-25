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
        Task<Bid> GetBidById(Guid userId, Guid bidId);
        IQueryable<Bid> GetUserBids(Guid userId);
        IQueryable<Bid> GetItemBids(Guid id);
        Task CreateBid(CreateBidCommand command, CancellationToken cancellationToken);
        Task UpdateBid(Bid bid, UpdateBidCommand command, Guid userId, CancellationToken cancellationToken);
        Task DeleteBid(Bid bid, CancellationToken cancellationToken);
    }
}