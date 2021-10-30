using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AskService : IAskService
    {
        private readonly IApplicationDbContext _context;

        public AskService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Ask> GetAskByIdAsync(Guid askId)
        {
            var ask = await _context.Asks
                .Include(x => x.Item)
                .Include(x => x.Size)
                .FirstOrDefaultAsync(x => x.Id == askId);

            return ask;
        }

        public async Task<List<Ask>> GetUserAsks(Guid userId)
        {
            var asks = await _context.Asks
                .Include(x => x.Item)
                    .ThenInclude(y => y.Bids)
                .Include(x => x.Size)
                .Where(x => x.CreatedBy == userId)
                .ToListAsync();

            return asks;
        }

        public async Task<List<Ask>> GetItemAsks(Guid itemId)
        {
            var asks = await _context.Asks
                .Include(x => x.Item)
                .Include(x => x.Size)
                .Where(x => x.ItemId == itemId)
                .ToListAsync();

            return asks;
        }

        public async Task CreateAskAsync(CreateAskCommand command, decimal fee, CancellationToken cancellationToken)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Value == command.Size, cancellationToken);
            
            var ask = new Ask()
            {
                ItemId = Guid.Parse(command.ItemId),
                Size = size,
                Price = decimal.Parse(command.Price),
                SellerFee = fee
            };

            await _context.Asks.AddAsync(ask, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAskAsync(Ask ask, UpdateAskCommand command, decimal fee, CancellationToken cancellationToken)
        {
            var size = await _context.Sizes.FirstOrDefaultAsync(x => x.Value == command.Size);
            
            ask.Price = Decimal.Parse(command.Price);
            ask.Size = size;
            ask.SellerFee = fee;
            
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        public async Task DeleteAskAsync(Ask ask, CancellationToken cancellationToken)
        {
            _context.Asks.Remove(ask);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}