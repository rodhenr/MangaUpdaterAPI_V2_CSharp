using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class MangaSourceConfiguration: IEntityTypeConfiguration<MangaSource>
{
    public void Configure(EntityTypeBuilder<MangaSource> builder)
    {
        builder.HasKey(p => new { p.MangaId, p.SourceId });
    }
}
