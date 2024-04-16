using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data.Entities;
using MangaUpdater.Data;
using MangaUpdater.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class GenreRepositoryTests
{
    private readonly AppDbContextIdentity _context;
    private readonly IGenreRepository _repository;

    public GenreRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<AppDbContextIdentity>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new AppDbContextIdentity(dbOptions.Options);
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
            new() { Id = 1, Name = "Genre1" }
        };

        _context.Genres.AddRange(sampleGenre);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetGenresByListIdAsync_Should_Return_List_Genre()
    {
        // Arrange
        var sampleGenre = new List<Genre>
        {
            new() { Id = 1, Name = "Genre1" },
            new() { Id = 2, Name = "Genre2" },
            new() { Id = 3, Name = "Genre3" },
            new() { Id = 4, Name = "Genre4" },
        };

        _context.Genres.AddRange(sampleGenre);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetGenresByListIdAsync(new List<int> { 1, 2 });

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Create_Should_Return_Create_Genre()
    {
        // Arrange
        var sampleGenre = new Genre { Id = 1, Name = "Genre1" };

        // Act
        _repository.Create(sampleGenre);
        await _context.SaveChangesAsync();

        // Assert
        var genreList = _context.Genres.ToList();
        genreList.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetAsync_Should_Return_List_Genre()
    {
        // Arrange
        var sampleGenre = new List<Genre>
        {
            new() { Id = 1, Name = "Genre1" },
            new() { Id = 2, Name = "Genre2" },
            new() { Id = 3, Name = "Genre3" },
            new() { Id = 4, Name = "Genre4" },
        };

        _context.Genres.AddRange(sampleGenre);
        await _repository.SaveAsync();

        // Act
        var result = await _repository.GetAsync();

        // Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task Update_Should_Update_Genre()
    {
        // Arrange
        var expectedGenre = new Genre { Id = 1, Name = "ModifiedGenre1" };
        var sampleGenre = new List<Genre>
        {
            new() { Id = 1, Name = "Genre1" },
            new() { Id = 2, Name = "Genre2" },
            new() { Id = 3, Name = "Genre3" }
        };

        _context.Genres.AddRange(sampleGenre);
        await _context.SaveChangesAsync();

        sampleGenre.Where(g => g.Id == 1)!.First().Name = "ModifiedGenre1";

        // Act
        _repository.Update(sampleGenre.Where(g => g.Id == 1)!.First());
        await _context.SaveChangesAsync();

        // Assert
        var firstGenre = _context.Genres.ToList().Where(g => g.Id == 1)!.First();
        firstGenre.Should().BeEquivalentTo(expectedGenre);
    }
}