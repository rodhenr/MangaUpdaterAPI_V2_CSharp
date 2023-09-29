using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserMangaChapterServiceTests
{
    private readonly Mock<IUserMangaRepository> _userMangaRepository;
    private readonly Mock<IChapterRepository> _chapterRepository;
    private readonly UserMangaChapterService _service;

    public UserMangaChapterServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _userMangaRepository = new Mock<IUserMangaRepository>();
        _chapterRepository = new Mock<IChapterRepository>();
        _service = new UserMangaChapterService(_userMangaRepository.Object, _chapterRepository.Object, mapper);
    }

    [Fact]
    public async Task Add_Should_Call_Create_Method_In_Repository()
    {
        // Arrange
        var sampleUserManga = new List<UserManga>()
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 5 },
            new() { Id = 1, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 10 },
            new() { Id = 1, MangaId = 1, SourceId = 3, UserId = "1", CurrentChapterId = 15 },
        };

        var sampleChapter = new Chapter
            { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = 7 };

        _userMangaRepository
            .Setup(repo => repo.GetAllByMangaIdAndUserIdAsync(1, "1"))
            .ReturnsAsync(sampleUserManga);

        _chapterRepository.Setup(repo => repo.GetSmallestChapterByMangaIdAsync(1, 1))
            .ReturnsAsync(sampleChapter);

        // Act
        await _service.AddUserManga(1, "1", 1);

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByMangaIdAndUserIdAsync(1, "1"), Times.Once);
        _chapterRepository.Verify(repo => repo.GetSmallestChapterByMangaIdAsync(1, 1), Times.Once);
        _userMangaRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}