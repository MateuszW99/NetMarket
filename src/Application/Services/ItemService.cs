﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Commands;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ItemService : IItemService
    {
        private readonly IApplicationDbContext _context;
        private readonly string _currentUserId;

        public ItemService(IApplicationDbContext context, IHttpService httpService)
        {
            _context = context;
            _currentUserId = httpService.GetUserId();
        }

        public async Task CreateItemAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Name == command.Brand, cancellationToken: cancellationToken);

            if (brand == null)
            {
                var newBrand = new Brand()
                {
                    Name = command.Brand
                };

                await _context.Brands.AddAsync(newBrand, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                brand = newBrand;
            }

            var item = new Item()
            {
                Brand = brand,
                Make = command.Make,
                Model = command.Model,
                Gender = command.Gender,
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

            if (item != null)
            {
                item.Asks = await GetItemAsks(item.Id);
                item.Bids = await GetItemBids(item.Id);
            }


            if (item != null)
            {
                item.Asks = await GetItemAsks(item.Id);
                item.Bids = await GetItemBids(item.Id);
            }


            return item;
        }

        public async Task<List<Item>> GetItemsWithQuery(SearchItemsQuery query)
        {
            var itemsQuery = _context.Items
                .Include(x => x.Brand)
                .OrderBy(x => x.Name)
                .AsQueryable();

            return await FilterItems(itemsQuery, query);
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

        public IQueryable<Item> GetTrendingItems(string category, int count)
        {
            var itemsQuery = _context.Items
                .Where(X => X.Category == category)
                .OrderBy(x => Guid.NewGuid())
                .Take(count)
                .AsQueryable();

            return itemsQuery;
        }

        public async Task UpdateItemAsync(Item item, UpdateItemCommand command, CancellationToken cancellationToken)
        {
            item.Name = command.Name;
            item.Description = command.Description;
            item.Make = command.Make;
            item.Model = command.Model;
            item.Gender = command.Gender;
            item.ImageUrl = command.ImageUrl;
            item.SmallImageUrl = command.SmallImageUrl;
            item.ThumbUrl = command.ThumbUrl;
            item.RetailPrice = command.RetailPrice;
            item.Category = command.Category;

            if (item.BrandId != Guid.Parse(command.Brand.Id))
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Brand.Id),
                    cancellationToken);
                item.Brand = brand;
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteItemAsync(Item item, CancellationToken cancellationToken)
        {
            _context.Items.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task<List<Item>> FilterItems(IQueryable<Item> itemsQuery, SearchItemsQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
            {
                itemsQuery = itemsQuery.Where(x => x.Name.ToLower().Contains(query.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Category))
            {
                itemsQuery = itemsQuery.Where(x => x.Category.ToLower().Contains(query.Category.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Make))
            {
                itemsQuery = itemsQuery.Where(x => x.Make.ToLower().Contains(query.Make.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Model))
            {
                itemsQuery = itemsQuery.Where(x => x.Model.ToLower().Contains(query.Model.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Gender))
            {
                itemsQuery = itemsQuery.Where(x => x.Gender.ToLower().Contains(query.Gender.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.Brand))
            {
                itemsQuery = itemsQuery.Where(x => x.Brand.Name.ToLower().Contains(query.Brand.ToLower()));
            }

            var items = await itemsQuery.ToListAsync();

            foreach (var item in items)
            {
                item.Asks = await GetItemAsks(item.Id);
                item.Bids = await GetItemBids(item.Id);
            }

            if (string.IsNullOrEmpty(query.MinPrice) && string.IsNullOrEmpty(query.MaxPrice))
            {
                return items;
            }

            return await FilterItemsByPrice(items,
                string.IsNullOrEmpty(query.MinPrice) ? Decimal.MinValue : Convert.ToDecimal(query.MinPrice),
                string.IsNullOrEmpty(query.MaxPrice) ? Decimal.MaxValue : Convert.ToDecimal(query.MaxPrice));
        }

        private async Task<List<Item>> FilterItemsByPrice(List<Item> items, decimal minPrice, decimal maxPrice)
        {
            items = items.Where(x => x.Asks != null).ToList();

            for (var i = items.Count - 1; i >= 0; i--)
            {
                var asks = items[i].Asks
                    .Where(x => minPrice <= x.Price && maxPrice >= x.Price)
                    .ToList();

                if (!asks.Any())
                {
                    items.Remove(items[i]);
                    continue;
                }

                items[i].Asks = asks;
            }

            return await Task.FromResult(items);
        }

        public async Task<List<Ask>> GetItemAsks(Guid itemId)
        {
            var query = _context.Asks.Where(x => x.ItemId == itemId).AsNoTracking();

            if (string.IsNullOrEmpty(_currentUserId))
            {
                return await query.ToListAsync();
            }

            return await query.Where(x => x.CreatedBy != Guid.Parse(_currentUserId))
                .ToListAsync();
        }

        public async Task<List<Bid>> GetItemBids(Guid itemId)
        {
            var query = _context.Bids.Where(x => x.ItemId == itemId).AsNoTracking();

            if (string.IsNullOrEmpty(_currentUserId))
            {
                return await query.ToListAsync();
            }

            return await query.Where(x => x.CreatedBy != Guid.Parse(_currentUserId))
                .ToListAsync();
        }
    }
}