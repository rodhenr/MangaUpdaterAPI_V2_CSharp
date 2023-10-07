using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class MangaAuthorRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IMangaAuthorRepository _repository;

    public MangaAuthorRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new MangaAuthorRepository(_context);
    }

    [Fact]
    public async Task BulkCreate_Should_Add_MangaAuthors()
    {
        // Arrange
        var sampleMangaAuthors = new List<MangaAuthor>
        {
            new() { Id = 1, MangaId = 1, Name = "Author1" },
            new() { Id = 2, MangaId = 2, Name = "Author1" },
            new() { Id = 3, MangaId = 3, Name = "Author1" },
            new() { Id = 4, MangaId = 4, Name = "Author1" },
        };

        // Act
        _repository.BulkCreate(sampleMangaAuthors);
        await _context.SaveChangesAsync();

        // Assert
        var addMangaAuthors = _context.MangaAuthors.ToList();
        Assert.Equal(4, addMangaAuthors.Count);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_MangaAuthor()
    {
        // Arrange
        var expectedMangaAuthor = new MangaAuthor { Id = 1, MangaId = 1, Name = "Author1" };
        var sampleMangaAuthor = new List<MangaAuthor>
        {
            expectedMangaAuthor,
            new() { Id = 2, MangaId = 2, Name = "Author1" },
            new() { Id = 3, MangaId = 3, Name = "Author1" },
            new() { Id = 4, MangaId = 4, Name = "Author1" },
        };

        _context.MangaAuthors.AddRange(sampleMangaAuthor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedMangaAuthor);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null()
    {
        // Arrange
        var sampleMangaAuthor = new List<MangaAuthor>
        {
            new() { Id = 1, MangaId = 1, Name = "Author1" },
            new() { Id = 2, MangaId = 2, Name = "Author1" },
            new() { Id = 3, MangaId = 3, Name = "Author1" },
            new() { Id = 4, MangaId = 4, Name = "Author1" },
        };

        _context.MangaAuthors.AddRange(sampleMangaAuthor);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(5);

        // Assert
        Assert.Null(result);
    }
}