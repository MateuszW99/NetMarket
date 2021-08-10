using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        
        public PaginatedList(List<T> items, int pageIndex, int totalCount, int pageSize)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            TotalCount = totalCount;
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.ToListAsync();
            return new PaginatedList<T>(items, pageIndex, count, pageSize);
        }

        public static PaginatedList<T> Create(IList<T> source, int pageIndex, int pageSize)
        {
            var items = source.ToList();
            return new PaginatedList<T>(items, pageIndex, items.Count(), pageSize);
        }
    }
}