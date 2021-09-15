using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    public static class DbHelper
    {
        public static async Task<T> FindAsync<T>(CustomWebApplicationFactory factory, Guid id) where T : class
        {
            var context = GetDbContext(factory);
            return await context.FindAsync<T>(id);
        }
        
        public static ApplicationDbContext GetDbContext(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public static void DeleteDatabase(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
        }
    }
}