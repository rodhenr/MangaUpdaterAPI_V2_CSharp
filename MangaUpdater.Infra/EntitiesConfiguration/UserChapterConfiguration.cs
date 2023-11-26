using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class UserChapterConfiguration : IEntityTypeConfiguration<UserChapter>
{
    public void Configure(EntityTypeBuilder<UserChapter> builder)
    {
        builder
            .HasIndex(us => new { us.UserMangaId, us.SourceId })
            .IsUnique();

        builder
            .HasOne(uc => uc.UserManga)
            .WithMany(um => um.UserChapter)
            .HasForeignKey(uc => uc.UserMangaId);

        builder
            .HasOne(us => us.Source)
            .WithMany(us => us.UserChapter)
            .HasForeignKey(us => us.SourceId);

        builder
            .HasOne(uc => uc.Chapter)
            .WithMany(um => um.UserChapter)
            .HasForeignKey(uc => uc.ChapterId);
    }
}