using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class UserMangaConfiguration : IEntityTypeConfiguration<UserManga>
{
    public void Configure(EntityTypeBuilder<UserManga> builder)
    {
        builder.HasKey(p => new { p.MangaId, p.UserId, p.SourceId });
        builder.HasOne(a => a.Manga).WithMany(a => a.UserMangas).HasForeignKey(a => a.MangaId);
        builder.HasOne(a => a.Source).WithMany(a => a.UserMangas).HasForeignKey(a => a.SourceId);
    }
}