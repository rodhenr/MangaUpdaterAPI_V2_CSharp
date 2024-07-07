using MangaUpdater.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Infrastructure.Configurations;

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
            .HasFilter("IsMainTitle = 1")
            .IsUnique();
    }
}