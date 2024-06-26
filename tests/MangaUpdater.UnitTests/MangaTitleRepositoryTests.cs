﻿using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data.Entities;
using MangaUpdater.Data;
using MangaUpdater.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class MangaTitleRepositoryTests
{
    private readonly AppDbContextIdentity _context;
    private readonly IMangaTitleRepository _repository;

    public MangaTitleRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<AppDbContextIdentity>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new AppDbContextIdentity(dbOptions.Options);
        _repository = new MangaTitleRepository(_context);
    }
    
    [Fact]
    public async Task BulkCreate_Should_Add_MangaTitle()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleMangaTitle = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1"},
            new() { Id = 2, MangaId = 1, Name = "Title2" },
            new() { Id = 3, MangaId = 2, Name = "Title3" },
            new() { Id = 4, MangaId = 3, Name = "Title4" },
        };

        // Act
        _repository.BulkCreate(sampleMangaTitle);
        await _context.SaveChangesAsync();

        // Assert
        var addedMangaTitle = _context.MangaTitles.ToList();
        Assert.Equal(4, addedMangaTitle.Count);
    }
}