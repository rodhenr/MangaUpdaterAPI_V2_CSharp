using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class MangaGenreConfiguration: IEntityTypeConfiguration<MangaGenre>
{
    public void Configure(EntityTypeBuilder<MangaGenre> builder)
    {
        builder.HasKey(p => new { p.MangaId, p.GenreId });
        builder.HasOne(a => a.Manga).WithMany(a => a.MangaGenres).HasForeignKey(a => a.MangaId);
        builder.HasOne(a => a.Genre).WithMany(a => a.MangaGenres).HasForeignKey(a => a.GenreId);
    }
}
