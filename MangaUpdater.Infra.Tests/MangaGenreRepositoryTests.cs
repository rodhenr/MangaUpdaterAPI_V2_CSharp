using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class MangaGenreRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IMangaGenreRepository _repository;

    public MangaGenreRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new MangaGenreRepository(_context);
    }

    [Fact]
    public async Task BulkCreate_Should_Add_MangaGenres()
    {
        // Arrange
        var sampleMangaGenres = new List<MangaGenre>
        {
            new() { Id = 1, MangaId = 1, GenreId = 2 },
            new() { Id = 2, MangaId = 1, GenreId = 3 },
            new() { Id = 3, MangaId = 2, GenreId = 4 },
            new() { Id = 4, MangaId = 2, GenreId = 5 },
        };

        // Act
        _repository.BulkCreate(sampleMangaGenres);
        await _context.SaveChangesAsync();

        // Assert
        var addMangaGenres = _context.MangaGenres.ToList();
        Assert.Equal(4, addMangaGenres.Count);
    }
}