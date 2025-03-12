using System;
using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafia.Persistence.Configurations;

public class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Url).IsRequired();
        builder.Property(e => e.GameId).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.UploadedAt).IsRequired();
        builder.HasOne(e => e.Game).WithMany(e => e.Photos).HasForeignKey(e => e.GameId);
        builder.HasOne(e => e.User).WithMany(e => e.Photos).HasForeignKey(e => e.UserId);
    }
}
