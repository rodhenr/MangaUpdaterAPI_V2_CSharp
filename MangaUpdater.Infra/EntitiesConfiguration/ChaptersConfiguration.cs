using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class ChaptersConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(a => new { a.MangaId, a.SourceId, a.Number });
        builder.Property(a => a.Id).ValueGeneratedOnAdd();
        builder.HasOne(a => a.Manga).WithMany(a => a.Chapters).HasForeignKey(a => a.MangaId);
        builder.HasOne(a => a.Source).WithMany(a => a.Chapters).HasForeignKey(a => a.SourceId);
    }
}
