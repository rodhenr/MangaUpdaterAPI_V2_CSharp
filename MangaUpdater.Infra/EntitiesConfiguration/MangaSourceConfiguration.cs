using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class MangaSourceConfiguration: IEntityTypeConfiguration<MangaSource>
{
    public void Configure(EntityTypeBuilder<MangaSource> builder)
    {
        builder.HasKey(a => new { a.MangaId, a.SourceId });
        builder.HasOne(a => a.Manga).WithMany(a => a.MangaSources).HasForeignKey(a => a.MangaId);
        builder.HasOne(a => a.Source).WithMany(a => a.MangaSources).HasForeignKey(a => a.SourceId);
    }
}
