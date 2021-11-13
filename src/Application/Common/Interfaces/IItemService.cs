using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Commands;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IItemService
    {
        Task CreateItemAsync(CreateItemCommand command, CancellationToken cancellationToken);
        Task<Item> GetItemByIdAsync(Guid id);
        Task<List<Item>> GetItemsWithQuery(SearchItemsQuery query);
        IQueryable<Item> GetItemsWithCategory(string category);
        IQueryable<Item> GetTrendingItems(string category, int count);
        Task UpdateItemAsync(Item item, UpdateItemCommand command, CancellationToken cancellationToken);
        Task DeleteItemAsync(Item item, CancellationToken cancellationToken);
        Task<List<Ask>> GetItemAsks(Guid itemId);
        Task<List<Bid>> GetItemBids(Guid itemId);
    }
}