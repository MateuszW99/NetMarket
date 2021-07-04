using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        private readonly IDomainEventService _domainEventService;
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDomainEventService domainEventService) 
            : base(options)
        {
            _domainEventService = domainEventService;
        }
        
        public DbSet<Ask> Asks { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemSize> ItemSizes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            // TODO : act in case of faulted task
            await DispatchEvents();
            return result;
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker
                    .Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(e => !e.IsPublished);

                if (domainEventEntity == null)
                {
                    break;
                }

                domainEventEntity.IsPublished = true;
                await _domainEventService.Publish(domainEventEntity);
            }
        }
    }
}