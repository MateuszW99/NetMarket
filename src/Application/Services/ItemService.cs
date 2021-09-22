﻿using System;
using System.Globalization;
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
                Name = command.Name,
                Category = command.Category
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

        public IQueryable<Item> GetItemsWithQuery(SearchItemsQuery query)
        {
            var itemsQuery = _context.Items
                .Include(x => x.Brand)
                .OrderBy(x => x.Name)
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Name))
            {
                itemsQuery = itemsQuery.Where(x => x.Name.Contains(query.Name));
            }
            
            if (!string.IsNullOrEmpty(query.Category))
            {
                itemsQuery = itemsQuery.Where(x => x.Category.Contains(query.Category));
            }
            
            if (!string.IsNullOrEmpty(query.Make))
            {
                itemsQuery = itemsQuery.Where(x => x.Make.Contains(query.Make));
            }
            
            if (!string.IsNullOrEmpty(query.Model))
            {
                itemsQuery = itemsQuery.Where(x => x.Model.Contains(query.Model));
            }
            
            if (!string.IsNullOrEmpty(query.Brand))
            {
                itemsQuery = itemsQuery.Where(x => x.Brand.Name.Contains(query.Brand));
            }
            
            if (!string.IsNullOrEmpty(query.MinPrice))
            {
                itemsQuery = itemsQuery.Where(x => x.RetailPrice >= Convert.ToDecimal(query.MinPrice, new CultureInfo("en-US")));
            }
            
            if (!string.IsNullOrEmpty(query.MaxPrice))
            {
                itemsQuery = itemsQuery.Where(x => x.RetailPrice <= Convert.ToDecimal(query.MaxPrice, new CultureInfo("en-US")));
            }
            
            return itemsQuery;
        }

        public IQueryable<Item> GetItemsWithCategory(string category)
        {
            var itemsQuery = _context.Items
                .Include(x => x.Brand)
                .OrderBy(x => x.Name)
                .Where(X => X.Category == category)
                .AsQueryable();

            return itemsQuery;
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
            item.Category = command.Category;
            
            if (item.BrandId != Guid.Parse(command.Brand.Id))
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Brand.Id), cancellationToken);
                item.Brand = brand;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}