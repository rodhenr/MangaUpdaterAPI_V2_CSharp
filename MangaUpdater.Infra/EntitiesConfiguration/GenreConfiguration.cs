using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder
            .Property(e => e.Id)
            .ValueGeneratedNever();

        builder
            .Property(g => g.Name)
            .HasMaxLength(20);
    }
}