using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(e => e.Make)
                .IsRequired(true)
                .HasMaxLength(150);
            
            builder.Property(e => e.Model)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(e => e.RetailPrice)
                .HasDefaultValue(0.0)
                .HasColumnType("money");

            builder.Property(e => e.Description)
                .IsRequired(false)
                .HasMaxLength(1500);

            builder.Property(e => e.ImageUrl)
                .IsRequired(false);
            
            builder.Property(e => e.SmallImageUrl)
                .IsRequired(false);
            
            builder.Property(e => e.ThumbUrl)
                .IsRequired(false);

            builder.Property(e => e.BrandId)
                .IsRequired();
            builder.HasOne(e => e.Brand);
        }
    }
}