﻿using System;
using System.Collections.Generic;
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
        Task<List<Item>> GetItemsAsync(SearchItemsQuery query, int pageSize, int pageIndex);
        Task UpdateItemAsync(UpdateItemCommand command, CancellationToken cancellationToken);
    }
}