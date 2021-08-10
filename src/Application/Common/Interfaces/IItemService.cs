using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IItemService
    {
        Task<Item> GetItemById(Guid id);
        Task<List<Item>> GetItems(SearchItemsQuery query, int pageSize, int pageIndex);
    }
}