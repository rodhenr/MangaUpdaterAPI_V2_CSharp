using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class ChaptersConfiguration : IEntityTypeConfiguration<Chapter>
{
    public void Configure(EntityTypeBuilder<Chapter> builder)
    {
        builder.HasKey(p => new { p.MangaId, p.SourceId, p.Number });
        builder.HasOne(p => p.Manga).WithMany(m => m.Chapters).HasForeignKey(p => p.MangaId);
    }
}
