using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Data.Entities.Configurations;

public class ChaptersConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder
            .HasIndex(ch => new { ch.MangaId, ch.SourceId, ch.Number })
            .IsUnique();

        builder.HasOne(ch => ch.Manga)
            .WithMany(ch => ch.Chapters)
            .HasForeignKey(ch => ch.MangaId);

        builder.HasOne(ch => ch.Source)
            .WithMany(ch => ch.Chapters)
            .HasForeignKey(ch => ch.SourceId);
    }
}