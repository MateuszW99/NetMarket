﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class AskConfiguration : IEntityTypeConfiguration<Ask>
    {
        public void Configure(EntityTypeBuilder<Ask> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.ItemSize);
            
            builder.Property(e => e.Price)
                .HasColumnType("money");
            
            builder.Property(e => e.IsCanceled)
                .HasDefaultValue(false);
        }
    }
}