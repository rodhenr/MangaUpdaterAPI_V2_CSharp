using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Data.Entities.Configurations;

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