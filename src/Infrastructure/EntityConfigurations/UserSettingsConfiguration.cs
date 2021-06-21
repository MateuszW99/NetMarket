using System;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class UserSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.SellerLevel)
                .HasDefaultValue(SellerLevel.Beginner)
                .HasConversion<string>();

            builder.Property(e => e.SalesCompleted)
                .HasDefaultValue(0)
                .HasMaxLength(Int32.MaxValue);

            builder.Property(e => e.PaypalEmail)
                .HasMaxLength(40);

            builder.Property(e => e.BillingStreet)
                .HasMaxLength(50);
            
            builder.Property(e => e.BillingAddressLine1)
                .HasMaxLength(50);
            
            builder.Property(e => e.BillingAddressLine2)
                .HasMaxLength(50);
            
            builder.Property(e => e.BillingZipCode)
                .HasMaxLength(10);
            
            builder.Property(e => e.BillingCountry)
                .HasMaxLength(50);
            
            builder.Property(e => e.ShippingStreet)
                .HasMaxLength(50);
            
            builder.Property(e => e.ShippingAddressLine1)
                .HasMaxLength(50);
            
            builder.Property(e => e.ShippingAddressLine2)
                .HasMaxLength(50);
            
            builder.Property(e => e.ShippingZipCode)
                .HasMaxLength(10);
            
            builder.Property(e => e.ShippingCountry)
                .HasMaxLength(50);
        }
    }
}