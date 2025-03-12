using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafia.Persistence.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.ProductId).IsRequired();
        builder.Property(e => e.Quantity).IsRequired();
        builder.Property(e => e.AddedAt).IsRequired();
        builder.HasOne(e => e.User).WithMany(e => e.Carts).HasForeignKey(e => e.UserId);
        builder.HasOne(e => e.Product).WithMany(e => e.Carts).HasForeignKey(e => e.ProductId);
    }
}
