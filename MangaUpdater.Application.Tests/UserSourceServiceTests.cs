using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserSourceServiceTests
{
    private readonly Mock<IUserMangaRepository> _userMangaRepository;
    private readonly Mock<IMangaSourceRepository> _mangaSourceRepository;
    private readonly UserSourceService _service;

    public UserSourceServiceTests()
    {
        _userMangaRepository = new Mock<IUserMangaRepository>();
        _mangaSourceRepository = new Mock<IMangaSourceRepository>();
        _service = new UserSourceService(_userMangaRepository.Object, _mangaSourceRepository.Object);
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
        var userMangaSample = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
        };
        var expectedDto = new List<UserSourceDto>
        {
            new(1, "Source1", true),
            new(2, "Source2", false),
            new(3, "Source3", false),
        };

        _mangaSourceRepository
            .Setup(repo => repo.GetAllByMangaIdAsync(1))
            .ReturnsAsync(mangaSourceSample);
        _userMangaRepository
            .Setup(repo => repo.GetAllByMangaIdAndUserIdAsync(1, "1"))
            .ReturnsAsync(userMangaSample);

        // Act
        var result = await _service.GetUserSourcesByMangaId(1, "1");

        // Assert
        _mangaSourceRepository.Verify(repo => repo.GetAllByMangaIdAsync(1), Times.Once);
        _userMangaRepository.Verify(repo => repo.GetAllByMangaIdAndUserIdAsync(1, "1"), Times.Once);
        result.Should().BeEquivalentTo(expectedDto);
    }
}