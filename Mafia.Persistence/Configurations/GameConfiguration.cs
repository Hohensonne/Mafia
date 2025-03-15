using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafia.Persistence.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EndOfRegistration).IsRequired();
        builder.Property(e => e.CurrentPlayers).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();
        builder.HasMany(e => e.GameRegistrations).WithOne(e => e.Game).HasForeignKey(e => e.GameId);
    }
}
