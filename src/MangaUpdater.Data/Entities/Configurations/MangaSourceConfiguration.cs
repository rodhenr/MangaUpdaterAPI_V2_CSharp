using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Data.Entities.Configurations;

public class MangaSourceConfiguration : IEntityTypeConfiguration<MangaSource>
{
    public void Configure(EntityTypeBuilder<MangaSource> builder)
    {
        builder
            .HasKey(ms => new { ms.MangaId, ms.SourceId });

        builder
            .Property(ms => ms.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(ms => ms.Manga)
            .WithMany(a => a.MangaSources)
            .HasForeignKey(a => a.MangaId);

        builder
            .HasOne(ms => ms.Source)
            .WithMany(a => a.MangaSources)
            .HasForeignKey(a => a.SourceId);

        builder
            .Property(ms => ms.Url)
            .HasMaxLength(100);
    }
}