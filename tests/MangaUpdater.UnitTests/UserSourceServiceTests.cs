using MangaUpdater.Core.Dtos;
using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserSourceServiceTests
{
    private readonly Mock<IUserChapterRepository> _userChapterRepository;
    private readonly Mock<IMangaSourceRepository> _mangaSourceRepository;
    private readonly UserSourceService _service;

    public UserSourceServiceTests()
    {
        _userChapterRepository = new Mock<IUserChapterRepository>();
        _mangaSourceRepository = new Mock<IMangaSourceRepository>();
        _service = new UserSourceService(_mangaSourceRepository.Object, _userChapterRepository.Object);
    }

    [Fact]
    public async Task GetUserSourcesByMangaId_Should_Return_A_List_Of_UserSourceDto()
    {
        // Arrange
        var mangaSourceSample = new List<MangaSource>
        {
            new()
            {
                Id = 1,
                MangaId = 1,
                SourceId = 1,
                Url = "url",
                Source = new Source { Id = 1, Name = "Source1", BaseUrl = "url" }
            },
            new()
            {
                Id = 2,
                MangaId = 1,
                SourceId = 2,
                Url = "url",
                Source = new Source { Id = 2, Name = "Source2", BaseUrl = "url" }
            },
            new()
            {
                Id = 3,
                MangaId = 1,
                SourceId = 3,
                Url = "url",
                Source = new Source { Id = 3, Name = "Source3", BaseUrl = "url" }
            },
        };
        var userMangaSample = new List<UserChapter>
        {
            new() { Id = 1, UserMangaId = 1, SourceId = 1, ChapterId = 1 },
        };
        var expectedDto = new List<UserSourceDto>
        {
            new(1, "Source1", true),
            new(2, "Source2", false),
            new(3, "Source3", false),
        };

        _mangaSourceRepository
            .Setup(repo => repo.GetAllByMangaIdAsync(It.IsAny<int>()))
            .ReturnsAsync(mangaSourceSample);
        _userChapterRepository
            .Setup(repo => repo.GetByUserMangaIdAsync(It.IsAny<int>()))
            .ReturnsAsync(userMangaSample);

        // Act
        var result = await _service.GetUserSourcesByMangaId(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        _mangaSourceRepository.Verify(repo => repo.GetAllByMangaIdAsync(It.IsAny<int>()), Times.Once);
        _userChapterRepository.Verify(repo => repo.GetByUserMangaIdAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeEquivalentTo(expectedDto);
    }
}