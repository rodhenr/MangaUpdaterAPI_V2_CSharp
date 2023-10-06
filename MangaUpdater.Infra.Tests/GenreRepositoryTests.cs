using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class GenreRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IGenreRepository _repository;

    public GenreRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new GenreRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Genre_If_Found()
    {
        // Arrange
        var expectedGenre = new Genre { Id = 1, Name = "Genre1" };
        var sampleGenre = new List<Genre>
        {
            expectedGenre,
            new() { Id = 2, Name = "Genre2" },
            new() { Id = 3, Name = "Genre3" }
        };

        _context.Genres.AddRange(sampleGenre);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedGenre, options =>
        {
            options.Excluding(s => s.MangaGenres);
            return options;
        });
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        var sampleGenre = new List<Genre>
        {
            new() { Id = 1, Name = "Genre1"}
        };

        _context.Genres.AddRange(sampleGenre);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.Null(result);
    }
}