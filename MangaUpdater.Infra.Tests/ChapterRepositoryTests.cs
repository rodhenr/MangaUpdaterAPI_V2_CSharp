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
    public async Task BulkCreate_Should_Add_Chapters()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleChapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = "2" },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = "3" },
            new() { Id = 4, MangaId = 2, SourceId = 1, Date = date, Number = "1" },
            new() { Id = 5, MangaId = 1, SourceId = 1, Date = date, Number = "4" },
            new() { Id = 6, MangaId = 3, SourceId = 1, Date = date, Number = "1" },
            new() { Id = 7, MangaId = 2, SourceId = 1, Date = date, Number = "2" },
        };

        // Act
        _repository.BulkCreate(sampleChapters);
        await _context.SaveChangesAsync();

        // Assert
        var addedChapters = _context.Chapters.ToList();
        Assert.Equal(7, addedChapters.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_Should_Return_Chapter_If_Found()
    {
        // Arrange
        var date = DateTime.Now;
        var source = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var expectedChapter = new Chapter
            { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1", Source = source };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = "2", Source = source },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = "3", Source = source }
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedChapter, options => options.Excluding(chap => chap.Source));
    }

    [Fact]
    public async Task GetLastChapterByMangaIdAndSourceIdAsync_Should_Return_Chapter()
    {
        // Arrange
        var date = DateTime.Now;
        var expectedChapter = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1" };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 2, SourceId = 1, Date = date, Number = "1" },
            new() { Id = 3, MangaId = 3, SourceId = 1, Date = date, Number = "1" },
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetLastChapterByMangaIdAndSourceIdAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedChapter, options => options.Excluding(chap => chap.Source));
    }

    [Fact]
    public async Task GetLastChapterByMangaIdAndSourceIdAsync_Should_Return_Null()
    {
        // Arrange
        var date = DateTime.Now;
        var expectedChapter = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1" };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 2, SourceId = 1, Date = date, Number = "1" },
            new() { Id = 3, MangaId = 3, SourceId = 1, Date = date, Number = "1" },
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetLastChapterByMangaIdAndSourceIdAsync(1, 2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetThreeLastByMangaIdAndSourceListAsync_Should_Return_List_Of_Chapters()
    {
        // Arrange
        var date = DateTime.Now;
        var source1 = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var source2 = new Source { Id = 2, Name = "Source2", BaseUrl = "url", Chapters = null };
        var sampleChapter = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1", Source = source1 },
            new() { Id = 2, MangaId = 1, SourceId = 2, Date = date, Number = "1", Source = source2 },
            new() { Id = 3, MangaId = 2, SourceId = 2, Date = date, Number = "1", Source = source2 },
            new() { Id = 4, MangaId = 2, SourceId = 1, Date = date, Number = "1", Source = source1 }
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetThreeLastByMangaIdAndSourceListAsync(1, new List<int> { 1, 2 });

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetThreeLastByMangaIdAndSourceListAsync_Should_Return_Empty_List()
    {
        // Arrange
        var date = DateTime.Now;
        var source1 = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var sampleChapter = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1", Source = source1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, Date = date, Number = "1", Source = source1 },
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetThreeLastByMangaIdAndSourceListAsync(1, new List<int> { 2 });

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetSmallestChapterByMangaIdAsync_Should_Return_Chapter_If_Found()
    {
        // Arrange
        var date = DateTime.Now;
        var source1 = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var source2 = new Source { Id = 2, Name = "Source2", BaseUrl = "url", Chapters = null };
        var expectedChapter = new Chapter
            { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1", Source = source1 };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = "2", Source = source1 },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = "3", Source = source1 },
            new() { Id = 4, MangaId = 1, SourceId = 1, Date = date, Number = "4", Source = source1 },
            new() { Id = 5, MangaId = 1, SourceId = 2, Date = date, Number = "1", Source = source2 }
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetSmallestChapterByMangaIdAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedChapter, options => options.Excluding(chap => chap.Source));
    }

    [Fact]
    public async Task GetSmallestChapterByMangaIdAsync_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        var date = DateTime.Now;
        var source1 = new Source { Id = 1, Name = "Source1", BaseUrl = "url", Chapters = null };
        var source2 = new Source { Id = 2, Name = "Source2", BaseUrl = "url", Chapters = null };
        var expectedChapter = new Chapter
            { Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1", Source = source1 };
        var sampleChapter = new List<Chapter>
        {
            expectedChapter,
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = "2", Source = source1 },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = "3", Source = source1 },
            new() { Id = 4, MangaId = 1, SourceId = 1, Date = date, Number = "4", Source = source1 },
            new() { Id = 5, MangaId = 2, SourceId = 2, Date = date, Number = "1", Source = source2 }
        };

        _context.Chapters.AddRange(sampleChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetSmallestChapterByMangaIdAsync(1, 2);

        // Assert
        Assert.Null(result);
    }
}