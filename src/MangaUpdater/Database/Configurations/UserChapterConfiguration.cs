using MangaUpdater.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Infrastructure.Configurations;

public class UserChapterConfiguration : IEntityTypeConfiguration<UserChapter>
{
    public void Configure(EntityTypeBuilder<UserChapter> builder)
    {
        builder
            .HasIndex(us => new { us.UserMangaId, us.SourceId })
            .IsUnique();
        
        builder
            .HasOne(uc => uc.UserManga)
            .WithMany(um => um.UserChapters)
            .HasForeignKey(uc => uc.UserMangaId);

        builder
            .HasOne(us => us.Source)
            .WithMany(us => us.UserChapters)
            .HasForeignKey(us => us.SourceId);

        builder
            .HasOne(uc => uc.Chapter)
            .WithMany(um => um.UserChapters)
            .HasForeignKey(uc => uc.ChapterId);
    }
}