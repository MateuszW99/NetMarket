﻿using System;
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

        public IQueryable<Ask> GetUserAsks(Guid userId)
        {
            var asks = _context.Asks
                .Include(x => x.Item)
                .Include(x => x.Size)
                .Where(x => x.CreatedBy == userId)
                .AsQueryable();

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
            var ask = new Ask()
            {
                ItemId = Guid.Parse(command.ItemId),
                SizeId = Guid.Parse(command.SizeId),
                Price = decimal.Parse(command.Price),
                SellerFee = fee
            };

            await _context.Asks.AddAsync(ask, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAskAsync(Ask ask, UpdateAskCommand command, decimal fee, CancellationToken cancellationToken)
        {
            ask.Price = Decimal.Parse(command.Price);
            ask.SizeId = Guid.Parse(command.SizeId);
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