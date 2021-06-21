using System.Security.Cryptography.X509Certificates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations
{
    internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Ignore(e => e.DomainEvents);
            
            builder.HasKey(e => e.Id);

            builder.Property(e => e.AskId)
                .IsRequired();
            builder.HasOne(e => e.Ask);
            
            builder.HasOne(e => e.Bid);
            builder.Property(e => e.BidId)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired(true)
                .HasConversion<string>();

            builder.Property(e => e.StartDate)
                .IsRequired();
            
            builder.Property(e => e.EndDate)
                .IsRequired();

            builder.Property(e => e.SellerFee)
                .HasColumnType("money");
            
            builder.Property(e => e.BuyerFee)
                .HasColumnType("money");

            builder.Property(e => e.Payout)
                .HasColumnType("money");
        }
    }
}