using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class MangaSourceRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IMangaSourceRepository _repository;

    public MangaSourceRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new MangaSourceRepository(_context);
    }

    [Fact]
    public async Task GetAllByMangaIdAsync_Should_Return_List_Of_MangaSource()
    {
        // Arrange
        var sampleMangaSourceList = new List<MangaSource>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Url = "Url" },
            new() { Id = 2, MangaId = 1, SourceId = 2, Url = "Url" },
            new() { Id = 3, MangaId = 2, SourceId = 1, Url = "Url" },
            new() { Id = 4, MangaId = 3, SourceId = 2, Url = "Url" },
        };
        var sampleMangaList = new List<Manga>()
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "", MyAnimeListId = 3 },
        };
        var sampleSourceList = new List<Source>()
        {
            new() { Id = 1, Name = "Source1", BaseUrl = "" },
            new() { Id = 2, Name = "Source2", BaseUrl = "" },
        };

        _context.MangaSources.AddRange(sampleMangaSourceList);
        _context.Mangas.AddRange(sampleMangaList);
        _context.Sources.AddRange(sampleSourceList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByMangaIdAsync(1);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByMangaIdAndSourceIdAsync_Should_Return_MangaSource()
    {
        // Arrange
        var expectedMangaSource = new MangaSource { Id = 1, MangaId = 1, SourceId = 1, Url = "Url" };
        var sampleMangaSourceList = new List<MangaSource>
        {
            expectedMangaSource,
            new() { Id = 2, MangaId = 1, SourceId = 2, Url = "Url" },
            new() { Id = 3, MangaId = 2, SourceId = 1, Url = "Url" },
        };
        var sampleMangaList = new List<Manga>()
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "", MyAnimeListId = 2 }
        };
        var sampleSourceList = new List<Source>()
        {
            new() { Id = 1, Name = "Source1", BaseUrl = "" },
            new() { Id = 2, Name = "Source2", BaseUrl = "" },
        };

        _context.MangaSources.AddRange(sampleMangaSourceList);
        _context.Mangas.AddRange(sampleMangaList);
        _context.Sources.AddRange(sampleSourceList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdAndSourceIdAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedMangaSource, options =>
        {
            options.Excluding(s => s.Source);
            options.Excluding(s => s.Manga);
            return options;
        });
    }
}