﻿using MangaUpdater.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaUpdater.Database.Configurations;

public class MangaAuthorConfiguration : IEntityTypeConfiguration<MangaAuthor>
{
    public void Configure(EntityTypeBuilder<MangaAuthor> builder)
    {
        builder
            .HasKey(ma => new { ma.MangaId, ma.Name });
        
        builder
            .Property(ma => ma.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(ma => ma.Manga)
            .WithMany(ma => ma.MangaAuthors)
            .HasForeignKey(ma => ma.MangaId);
    }
}