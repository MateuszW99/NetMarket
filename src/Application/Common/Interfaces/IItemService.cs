using System;
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
        IQueryable<Item> GetItemsWithQuery(SearchItemsQuery query);
        IQueryable<Item> GetItemsWithCategory(string category);
        Task UpdateItemAsync(UpdateItemCommand command, CancellationToken cancellationToken);
    }
}