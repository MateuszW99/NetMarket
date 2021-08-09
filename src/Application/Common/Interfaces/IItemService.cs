using System;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface IItemService
    {
        Task<Item> GetItemById(Guid id);
    }
}