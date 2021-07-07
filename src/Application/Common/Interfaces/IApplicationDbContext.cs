using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Ask> Asks { get; set; }
        DbSet<Bid> Bids { get; set; }
        DbSet<Brand> Brands { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<ItemSize> ItemSizes { get; set; }
        DbSet<Size> Sizes { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<UserSettings> UserSettings { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}