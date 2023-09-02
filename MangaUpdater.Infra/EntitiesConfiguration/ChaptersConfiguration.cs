using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class ChaptersConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder
            .HasKey(ch => new { ch.MangaId, ch.SourceId, ch.Number });

        builder
            .Property(ch => ch.Id)
            .ValueGeneratedOnAdd();

        builder.HasOne(ch => ch.Manga)
            .WithMany(ch => ch.Chapters)
            .HasForeignKey(ch => ch.MangaId);

        builder.HasOne(ch => ch.Source)
            .WithMany(ch => ch.Chapters)
            .HasForeignKey(ch => ch.SourceId);
    }
}