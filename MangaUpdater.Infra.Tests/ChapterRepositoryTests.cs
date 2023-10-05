using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class ChapterRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IChapterRepository _repository;

    public ChapterRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new ChapterRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnChapterIfFound()
    {
        // Arrange
        var date = DateTime.Now;
        var source = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var expectedChapter = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1" };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = "2", Source = source },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = "3", Source = source },
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedChapter, options => options.Excluding(chap => chap.Source));
    }
}