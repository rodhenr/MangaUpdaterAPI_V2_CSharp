using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

public class UserMangaConfiguration: IEntityTypeConfiguration<UserManga>
{
    public void Configure(EntityTypeBuilder<UserManga> builder)
    {
        builder.HasKey(p => new { p.MangaId, p.UserId });
    }
}