using Mafia.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mafia.Persistence.Configurations;

public class GameRegistrationConfiguration : IEntityTypeConfiguration<GameRegistration>
{
    public void Configure(EntityTypeBuilder<GameRegistration> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.GameId).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        builder.Property(e => e.CreatedAt).IsRequired();

    }
}
