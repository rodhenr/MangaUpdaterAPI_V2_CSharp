using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class ChapterServiceTests
{
    private readonly Mock<IChapterRepository> _repository;
    private readonly ChapterService _service;

    public ChapterServiceTests()
    {
        _repository = new Mock<IChapterRepository>();
        _service = new ChapterService(_repository.Object);
    } // TODO: Configure DI

    [Fact]
    public void BulkCreate_Chapters_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Arrange
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, Date = DateTime.Now, Number = 1 },
            new() { Id = 3, MangaId = 3, SourceId = 1, Date = DateTime.Now, Number = 1 }
        };

        // Act
        _service.BulkCreate(chapters);

        // Assert
        _repository.Verify(repo => repo.BulkCreate(chapters), Times.Once);
    }

    [Fact]
    public async Task GetLastByMangaIdAndSourceId_Should_Return_Last_Chapter()
    {
        // Arrange
        const int mangaId = 1;
        const int sourceId = 2;
        var expectedChapter = new Chapter
        {
            Id = 1, MangaId = mangaId, SourceId = sourceId, Date = DateTime.Now, Number = 3
        };

        _repository.Setup(repo => repo.GetLastChapterByMangaIdAndSourceIdAsync(mangaId, sourceId))
            .ReturnsAsync(expectedChapter);

        // Act
        var result = await _service.GetLastByMangaIdAndSourceId(mangaId, sourceId);

        // Assert
        _repository.Verify(repo => repo.GetLastChapterByMangaIdAndSourceIdAsync(mangaId, sourceId), Times.Once);
        Assert.Equal(expectedChapter, result);
    }
}