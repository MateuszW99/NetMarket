using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Bids.Commands;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface IBidService
    {
        Task<BidObject> GetBidById(Guid userId, Guid bidId);
        Task<PaginatedList<BidObject>> GetUserBids(Guid userId);
        Task CreateBid(CreateBidCommand command, Guid userId);
        Task UpdateBid(UpdateBidCommand command, Guid userId);
        Task DeleteBid(Guid bidId ,Guid userId);
    }
}