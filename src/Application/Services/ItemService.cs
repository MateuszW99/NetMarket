﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Commands;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        
        public ItemService(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateItemAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var brand = _context.Brands.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Brand.Id));
            
            var item = new Item()
            {
                Brand = await brand, 
                Make = command.Make,
                Model = command.Model,
                Description = command.Description,
                RetailPrice = command.RetailPrice,
                ImageUrl = command.ImageUrl,
                SmallImageUrl = command.SmallImageUrl,
                ThumbUrl = command.ThumbUrl,
                Name = command.Name
            };

            await _context.Items.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Item> GetItemByIdAsync(Guid id)
        {
            var item = await _context.Items
                .Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                throw new NotFoundException(nameof(Item), id);
            }
            
            return item;
        }

        public async Task<List<Item>> GetItemsAsync(SearchItemsQuery query, int pageSize, int pageIndex)
        {
            var items = await _context.Items.Where(x =>
                    (string.IsNullOrEmpty(query.Brand) && x.Brand.Name.Contains(query.Brand)) ||
                    (string.IsNullOrEmpty(query.Category) && x.Brand.Name.Contains(query.Category)) ||
                    (string.IsNullOrEmpty(query.Make) && x.Brand.Name.Contains(query.Make)) ||
                    (string.IsNullOrEmpty(query.Model) && x.Brand.Name.Contains(query.Model)) ||
                    (string.IsNullOrEmpty(query.Name) && x.Brand.Name.Contains(query.Name)))
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return items;
        }

        public async Task UpdateItemAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Id));
            
            if (item == null)
            {
                throw new NotFoundException(nameof(Item), command.Id);
            }
            
            item.Name = command.Name;
            item.Description = command.Description;
            item.Make = command.Make;
            item.Model = command.Model;
            item.ImageUrl = command.ImageUrl;
            item.SmallImageUrl = command.SmallImageUrl;
            item.ThumbUrl = command.ThumbUrl;
            item.RetailPrice = command.RetailPrice;
            
            if (item.BrandId != Guid.Parse(command.Brand.Id))
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Brand.Id), cancellationToken);
                item.Brand = brand;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}