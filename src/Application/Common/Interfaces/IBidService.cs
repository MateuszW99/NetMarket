﻿using System;
using System.Collections.Generic;
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
        Task<List<Bid>> GetUserBids(Guid userId);
        Task<List<Bid>> GetItemBids(Guid itemId);
        Task CreateBidAsync(CreateBidCommand command, decimal fee, CancellationToken cancellationToken);
        Task CreateBidAsync(Bid bid, CancellationToken cancellationToken);
        Task UpdateBidAsync(Bid bid, UpdateBidCommand command, decimal fee, CancellationToken cancellationToken);
        Task DeleteBidAsync(Bid bid, CancellationToken cancellationToken);
    }
}