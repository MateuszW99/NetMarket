using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
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

        public ItemService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateItemAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            var brand = _context.Brands.FirstOrDefaultAsync(x => x.Id == Guid.Parse(command.Brand.Id));
            
            var item = new Item()
            {
                Brand = await brand, 
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

            return item;
        }

        public IQueryable<Item> GetItemsWithQuery(SearchItemsQuery query)
        {
            var itemsQuery = _context.Items
                .Include(x => x.Brand)
                .Include(x => x.Asks)
                .OrderBy(x => x.Name)
                .AsQueryable();

            return FilterItems(itemsQuery, query);
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
            item.Gender = command.Gender;
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

        private static IQueryable<Item> FilterItems(IQueryable<Item> itemsQuery, SearchItemsQuery query)
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
            
            if (!string.IsNullOrEmpty(query.MinPrice))
            {
                var itemsWithMinPrice = new List<Item>();
                
                foreach (var item in itemsQuery)
                {
                    var itemAsks = item.Asks;
                    
                    if (itemAsks.Any())
                    {
                        if(itemAsks.Any(x => x.Price >= Convert.ToDecimal(query.MinPrice, new CultureInfo("en-US"))))
                        {
                            itemsWithMinPrice.Add(item);
                        }
                    }
                    else
                    {
                        if(item.RetailPrice >= Convert.ToDecimal(query.MinPrice, new CultureInfo("en-US")))
                        {
                            itemsWithMinPrice.Add(item);
                        }
                    }
                }

                itemsQuery = itemsQuery.Where(x => itemsWithMinPrice.Contains(x));
            }
            
            if (!string.IsNullOrEmpty(query.MaxPrice))
            {
                var itemsWithMaxPrice = new List<Item>();
                
                foreach (var item in itemsQuery)
                {
                    var itemAsks = item.Asks;
                    
                    if (itemAsks.Any())
                    {
                        if(itemAsks.Any(x => x.Price <= Convert.ToDecimal(query.MaxPrice, new CultureInfo("en-US"))))
                        {
                            itemsWithMaxPrice.Add(item);
                        }
                    }
                    else
                    {
                        if(item.RetailPrice <= Convert.ToDecimal(query.MaxPrice, new CultureInfo("en-US")))
                        {
                            itemsWithMaxPrice.Add(item);
                        }
                    }
                }

                itemsQuery = itemsQuery.Where(x => itemsWithMaxPrice.Contains(x));
                
            }
            
            return itemsQuery;
        }
    }
}