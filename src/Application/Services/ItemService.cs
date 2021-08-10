using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
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
        
        public async Task<Item> GetItemById(Guid id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                
            }

            return item;
        }

        public async Task<List<Item>> GetItems(SearchItemsQuery query, int pageSize, int pageIndex)
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
    }
}