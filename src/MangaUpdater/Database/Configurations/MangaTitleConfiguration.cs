using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Database.Configurations;

public class MangaTitleConfiguration : IEntityTypeConfiguration<MangaTitle>
{
    public void Configure(EntityTypeBuilder<MangaTitle> builder)
    {
        builder
            .Property(mt => mt.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(mt => mt.Manga)
            .WithMany(mt => mt.MangaTitles)
            .HasForeignKey(mt => mt.MangaId);

        builder
            .HasIndex(mt => new { mt.MangaId, mt.IsMyAnimeListMainTitle })
            .IsUnique()
            .HasFilter("IsMyAnimeListMainTitle = TRUE");
    }
}