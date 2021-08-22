using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface IBidService
    {
        Task<BidObject> GetBidById(Guid userId, Guid bidId);
        Task<PaginatedList<BidObject>> GetUserBids(Guid userId);
        Task CreateBid(Guid userId);
        Task UpdateBid(Guid userId);
        Task DeleteBid(Guid userId);
    }
}