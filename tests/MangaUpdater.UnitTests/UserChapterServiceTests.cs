using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserChapterServiceTests
{
    private readonly Mock<IUserChapterRepository> _repository;
    private readonly UserChapterService _service;

    public UserChapterServiceTests()
    {
        _repository = new Mock<IUserChapterRepository>();
        _service = new UserChapterService(_repository.Object);
    }

    [Fact]
    public async Task GetByUserMangaId_Should_Return_List_UserChapter()
    {
        // Arrange
        var sampleUserChapterList = new List<UserChapter>
        {
            new() { Id = 1, SourceId = 1, UserMangaId = 1, ChapterId = 1 },
            new() { Id = 3, SourceId = 2, UserMangaId = 1, ChapterId = 3 },
        };

        _repository
            .Setup(repo => repo.GetByUserMangaIdAsync(It.IsAny<int>()))
            .ReturnsAsync(sampleUserChapterList);

        // Act
        var result = await _service.GetByUserMangaId(It.IsAny<int>());

        // Assert
        _repository.Verify(repo => repo.GetByUserMangaIdAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeEquivalentTo(sampleUserChapterList);
    }
    
    [Fact]
    public async Task GetByUserMangaIdAndSourceIdAsync_Should_Return_UserChapter()
    {
        // Arrange
        var sampleUserChapter = new UserChapter { Id = 1, SourceId = 1, UserMangaId = 1, ChapterId = 1 };

        _repository
            .Setup(repo => repo.GetByUserMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleUserChapter);

        // Act
        var result = await _service.GetByUserMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        _repository.Verify(repo => repo.GetByUserMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        result.Should().BeEquivalentTo(sampleUserChapter);
    }
}