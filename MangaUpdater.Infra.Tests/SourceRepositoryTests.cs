using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class SourceRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly ISourceRepository _repository;

    public SourceRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new SourceRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Source_If_Found()
    {
        // Arrange
        var expectedSource = new Source { Id = 1, Name = "Source1", BaseUrl = "Url" };
        var sampleSource = new List<Source>
        {
            expectedSource,
            new() { Id = 2, Name = "Source2", BaseUrl = "Url" },
            new() { Id = 3, Name = "Source3", BaseUrl = "Url" }
        };

        _context.Sources.AddRange(sampleSource);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedSource, options =>
        {
            options.Excluding(s => s.Chapters);
            options.Excluding(s => s.MangaSources);
            options.Excluding(s => s.UserMangas);
            return options;
        });
    }
    
    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        var sampleSource = new List<Source>
        {
            new() { Id = 1, Name = "Source1", BaseUrl = "Url" }
        };

        _context.Sources.AddRange(sampleSource);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(2);

        // Assert
        Assert.Null(result);
    }
}