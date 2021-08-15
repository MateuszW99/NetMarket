using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static async Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
            this IQueryable<TDestination> queryable, int pageNumber, int pageSize) => 
               await PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);

        public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable source, IConfigurationProvider configuration)
        {
            return await Task.Run(() => source.ProjectTo<TDestination>(configuration).ToList());
        }
    }
}