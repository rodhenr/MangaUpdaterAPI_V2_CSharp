using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class UserMangaConfiguration : IEntityTypeConfiguration<UserManga>
{
    public void Configure(EntityTypeBuilder<UserManga> builder)
    {
        builder
            .HasKey(um => new { um.MangaId, um.UserId, um.SourceId });

        builder
            .Property(um => um.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(um => um.Manga)
            .WithMany(um => um.UserMangas)
            .HasForeignKey(um => um.MangaId);

        builder
            .HasOne(um => um.Source)
            .WithMany(um => um.UserMangas)
            .HasForeignKey(um => um.SourceId);

        builder
            .Property(um => um.UserId)
            .HasMaxLength(450);
    }
}