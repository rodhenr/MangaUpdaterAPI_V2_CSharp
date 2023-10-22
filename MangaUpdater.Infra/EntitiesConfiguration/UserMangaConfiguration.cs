using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class UserMangaConfiguration : IEntityTypeConfiguration<UserManga>
{
    public void Configure(EntityTypeBuilder<UserManga> builder)
    {
        builder
            .HasIndex(um => new { um.MangaId, um.UserId})
            .IsUnique();

        builder
            .HasOne(um => um.Manga)
            .WithMany(um => um.UserMangas)
            .HasForeignKey(um => um.MangaId);

        builder
            .Property(um => um.UserId)
            .HasMaxLength(450);
    }
}