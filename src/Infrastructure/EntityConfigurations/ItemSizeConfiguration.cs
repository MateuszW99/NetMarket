using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class ItemSizeConfiguration : IEntityTypeConfiguration<ItemSize>
    {
        public void Configure(EntityTypeBuilder<ItemSize> builder)
        {
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.Item);
            
            builder.HasOne(e => e.Size);
            
            builder.Property(e => e.SizeId)
                .IsRequired(false);
        }
    }
}