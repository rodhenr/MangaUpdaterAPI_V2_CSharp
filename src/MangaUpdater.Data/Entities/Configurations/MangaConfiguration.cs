using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Data.Entities.Configurations;

public class MangaConfiguration : IEntityTypeConfiguration<Manga>
{
    public void Configure(EntityTypeBuilder<Manga> builder)
    {
        builder
            .Property(m => m.CoverUrl)
            .HasMaxLength(200);

        builder
            .Property(m => m.Synopsis)
            .HasMaxLength(2000);

        builder
            .Property(m => m.Type)
            .HasMaxLength(50);
    }
}