using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class MangaGenreConfiguration : IEntityTypeConfiguration<MangaGenre>
{
    public void Configure(EntityTypeBuilder<MangaGenre> builder)
    {
        builder
            .HasKey(mg => new { mg.MangaId, mg.GenreId });

        builder
            .Property(mg => mg.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(mg => mg.Manga)
            .WithMany(mg => mg.MangaGenres)
            .HasForeignKey(mg => mg.MangaId);

        builder
            .HasOne(mg => mg.Genre)
            .WithMany(mg => mg.MangaGenres)
            .HasForeignKey(mg => mg.GenreId);
    }
}