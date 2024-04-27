using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Data.Entities.Configurations;

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder
            .Property(s => s.Name)
            .HasMaxLength(50);
        
        builder
            .Property(s => s.BaseUrl)
            .HasMaxLength(100);
    }
}