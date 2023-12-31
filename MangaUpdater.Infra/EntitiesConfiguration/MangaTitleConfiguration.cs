﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.EntitiesConfiguration;

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
            .HasIndex(mt => new { mt.MangaId, mt.IsMainTitle })
            .HasFilter("IsMainTitle = 1")
            .IsUnique();
    }
}