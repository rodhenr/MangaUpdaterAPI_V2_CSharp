using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
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
    public void BulkCreate_Should_Call_BulkCreate_Method_In_Repository()
    {
        // Arrange
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = "1" },
            new() { Id = 2, MangaId = 2, SourceId = 1, Date = DateTime.Now, Number = "1" },
            new() { Id = 3, MangaId = 3, SourceId = 1, Date = DateTime.Now, Number = "1" }
        };

        // Act
        _service.BulkCreate(chapters);

        // Assert
        _repository.Verify(repo => repo.BulkCreate(chapters), Times.Once);
    }

    [Fact]
    public async Task GetByIdAndMangaId_Should_Return_Chapter()
    {
        // Arrange
        var expected = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = "3" };

        _repository
            .Setup(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(expected);

        // Act
        var result = await _service.GetByIdAndMangaId(1, 1);

        // Assert
        _repository.Verify(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once);
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task GetLastByMangaIdAndSourceId_Should_Return_Last_Chapter()
    {
        // Arrange
        var expectedChapter = new Chapter
        {
            Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = "3"
        };

        _repository
            .Setup(repo => repo.GetLastChapterByMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(expectedChapter);

        // Act
        var result = await _service.GetLastByMangaIdAndSourceId(1, 1);

        // Assert
        _repository.Verify(repo => repo.GetLastChapterByMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once);
        Assert.Equal(expectedChapter, result);
    }
}