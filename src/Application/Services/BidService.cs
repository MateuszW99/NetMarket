using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BidService : IBidService
    {
        private readonly IApplicationDbContext _context;

        public BidService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Bid> GetBidByIdAsync(Guid bidId)
        {
            var bid = await _context.Bids
                .Include(x => x.Item)
                .Include(x => x.Size)
                .FirstOrDefaultAsync(x => x.Id == bidId);

            return bid;
        }

        public async Task<List<Bid>> GetUserBids(Guid userId)
        {
            var bids = await _context.Bids
                .Include(x => x.Item)
                    .ThenInclude(y => y.Asks)
                .Include(x => x.Size)
                .Where(x => x.CreatedBy == userId)
                .ToListAsync();

            return bids;
        }

        public async Task<List<Bid>> GetItemBids(Guid itemId)
        {
            var bids = await _context.Bids
                .Include(x => x.Item)
                .Include(x => x.Size)
                .Where(x => x.ItemId == itemId)
                .ToListAsync();

            return bids;
        }

        public async Task CreateBidAsync(CreateBidCommand command, decimal fee, CancellationToken cancellationToken)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Value == command.Size);
            
            var bid = new Bid()
            {
                ItemId = Guid.Parse(command.ItemId),
                Size = size,
                Price = decimal.Parse(command.Price),
                BuyerFee = fee
            };

            await _context.Bids.AddAsync(bid, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBidAsync(Bid bid, UpdateBidCommand command, decimal fee, CancellationToken cancellationToken)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Value == command.Size);
            
            bid.Price = Decimal.Parse(command.Price);
            bid.Size = size;
            bid.BuyerFee = fee;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBidAsync(Bid bid, CancellationToken cancellationToken)
        {
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}