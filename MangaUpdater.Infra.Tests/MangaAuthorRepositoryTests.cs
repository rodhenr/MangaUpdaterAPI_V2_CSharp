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
}