﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);
            
            builder.HasOne(e => e.Item);
            
            builder.HasOne(e => e.Size);
            
            builder.Property(e => e.Price)
                .HasColumnType("money"); 
            
            builder.Property(e => e.BuyerFee)
                .HasColumnType("money");

            builder.Property(e => e.UsedInTransaction).HasDefaultValue(false);
        }
    }
}