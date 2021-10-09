using System;
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

        public IQueryable<Bid> GetUserBids(Guid userId)
        {
            var bids = _context.Bids
                .Include(x => x.Item)
                .Include(x => x.Size)
                .Where(x => x.CreatedBy == userId)
                .AsQueryable();

            return bids;
        }

        public IQueryable<Bid> GetItemBids(Guid id)
        {
            var bids = _context.Bids
                .Include(x => x.Item)
                .Include(x => x.Size)
                .Where(x => x.ItemId == id)
                .OrderByDescending(x => x.Price)
                .AsQueryable();

            return bids;
        }

        public async Task CreateBidAsync(CreateBidCommand command, CancellationToken cancellationToken)
        {
            var bid = new Bid()
            {
                ItemId = Guid.Parse(command.ItemId),
                SizeId = Guid.Parse(command.SizeId),
                Price = decimal.Parse(command.Price)
            };

            await _context.Bids.AddAsync(bid, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateBidAsync(Bid bid, UpdateBidCommand command, Guid userId, CancellationToken cancellationToken)
        {
            bid.Price = Decimal.Parse(command.Price);
            bid.SizeId = Guid.Parse(command.SizeId);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteBidAsync(Bid bid, CancellationToken cancellationToken)
        {
            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}