using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserMangaChapterServiceTests
{
    private readonly Mock<IUserMangaRepository> _userMangaRepository;
    private readonly Mock<IChapterRepository> _chapterRepository;
    private readonly Mock<IUserSourceService> _userSourceService;
    private readonly UserMangaChapterService _service;

    public UserMangaChapterServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _userMangaRepository = new Mock<IUserMangaRepository>();
        _chapterRepository = new Mock<IChapterRepository>();
        _userSourceService = new Mock<IUserSourceService>();
        _service = new UserMangaChapterService(_userMangaRepository.Object, _chapterRepository.Object,
            _userSourceService.Object, mapper);
    }

    [Fact]
    public async Task AddUserMangaBySourceIdList_ShouldAddThreeUserMangas()
    {
        // Arrange
        var sampleChapter1 = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = 1 };
        var sampleChapter2 = new Chapter { Id = 2, MangaId = 1, SourceId = 2, Date = DateTime.Now, Number = 1 };
        var sampleChapter3 = new Chapter { Id = 3, MangaId = 1, SourceId = 3, Date = DateTime.Now, Number = 1 };
        var sampleChapter4 = new Chapter { Id = 4, MangaId = 1, SourceId = 4, Date = DateTime.Now, Number = 1 };
        var sampleUserSourceList = new List<UserSourceDto>
        {
            new(1, "Source1", false),
            new(2, "Source2", false),
            new(3, "Source3", false),
            new(4, "Source4", true)
        };
        var sourceList = new List<int> { 1, 2, 3, 4 };
        Func<UserManga, bool> userMangaMatchesExpectedValues1 =
            um => um is { UserId: "1", MangaId: 1, SourceId: 1, CurrentChapterId: 1 };
        Func<UserManga, bool> userMangaMatchesExpectedValues2 =
            um => um is { UserId: "1", MangaId: 1, SourceId: 2, CurrentChapterId: 2 };
        Func<UserManga, bool> userMangaMatchesExpectedValues3 =
            um => um is { UserId: "1", MangaId: 1, SourceId: 3, CurrentChapterId: 3 };

        _userSourceService
            .Setup(service => service.GetUserSourcesByMangaId(1, "1"))
            .ReturnsAsync(sampleUserSourceList);
        _chapterRepository.Setup(repo => repo.GetSmallestChapterByMangaIdAsync(1, 1))
            .ReturnsAsync(sampleChapter1);
        _chapterRepository.Setup(repo => repo.GetSmallestChapterByMangaIdAsync(1, 2))
            .ReturnsAsync(sampleChapter2);
        _chapterRepository.Setup(repo => repo.GetSmallestChapterByMangaIdAsync(1, 3))
            .ReturnsAsync(sampleChapter3);

        // Act
        await _service.AddUserMangaBySourceIdList(1, "1", sourceList);

        // Assert
        _userSourceService.Verify(service => service.GetUserSourcesByMangaId(1, "1"), Times.Once);
        _chapterRepository.Verify(repo => repo.GetSmallestChapterByMangaIdAsync(1, 1), Times.Once);
        _chapterRepository.Verify(repo => repo.GetSmallestChapterByMangaIdAsync(1, 2), Times.Once);
        _chapterRepository.Verify(repo => repo.GetSmallestChapterByMangaIdAsync(1, 3), Times.Once);
        _userMangaRepository.Verify(repo => repo.Create(It.Is<UserManga>(um => userMangaMatchesExpectedValues1(um))),
            Times.Once);
        _userMangaRepository.Verify(repo => repo.Create(It.Is<UserManga>(um => userMangaMatchesExpectedValues2(um))),
            Times.Once);
        _userMangaRepository.Verify(repo => repo.Create(It.Is<UserManga>(um => userMangaMatchesExpectedValues3(um))),
            Times.Once);
        _userMangaRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddUserMangaBySourceIdList_ShouldNotAddAnyUserManga()
    {
        // Arrange
        var sampleUserSourceList = Enumerable.Empty<UserSourceDto>();
        var sourceList = new List<int> { 1 };

        _userSourceService
            .Setup(service => service.GetUserSourcesByMangaId(1, "1"))
            .ReturnsAsync(sampleUserSourceList);

        // Act
        await _service.AddUserMangaBySourceIdList(1, "1", sourceList);

        // Assert
        _userSourceService.Verify(service => service.GetUserSourcesByMangaId(1, "1"), Times.Once);
        _chapterRepository.VerifyNoOtherCalls();
        _userMangaRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetUserMangasWithThreeLastChapterByUserId_Should_Return_List_Of_MangaUserLoggedDto()
    {
        // Arrange
        var date = DateTime.Now;
        var baseSource = new Source { Id = 1, Name = "Source1", BaseUrl = "url" };
        var userMangaSample = new List<UserManga>
        {
            new()
            {
                Id = 1,
                UserId = "1",
                MangaId = 1,
                SourceId = 1,
                CurrentChapterId = 2,
                Manga = new Manga
                {
                    Id = 1,
                    CoverUrl = "url",
                    Synopsis = "",
                    Type = "Manga",
                    MyAnimeListId = 1,
                    MangaTitles = new List<MangaTitle> { new() { Id = 1, MangaId = 1, Name = "Manga1" } }
                },
                Source = baseSource
            },
        };
        var chaptersSample = new List<Chapter>
        {
            new()
            {
                Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = 1, Source = baseSource
            },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = date, Number = 2, Source = baseSource },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = date, Number = 3, Source = baseSource }
        };
        var sourceListSample = new List<int> { 1 };
        var expectedMangaDto = new List<MangaUserLoggedDto>
        {
            new()
            {
                Id = 1,
                Name = "Manga1",
                CoverUrl = "url",
                Chapters = new List<ChapterDto>
                {
                    new() { ChapterId = 1, SourceId = 1, SourceName = "Source1", Date = date, Number = 1, Read = true },
                    new() { ChapterId = 2, SourceId = 1, SourceName = "Source1", Date = date, Number = 2, Read = true },
                    new()
                    {
                        ChapterId = 3, SourceId = 1, SourceName = "Source1", Date = date, Number = 3, Read = false
                    },
                }
            }
        };

        _userMangaRepository
            .Setup(repo => repo.GetAllByUserIdAsync("1"))
            .ReturnsAsync(userMangaSample);
        _chapterRepository
            .Setup(repo => repo.GetThreeLastByMangaIdAndSourceListAsync(1, sourceListSample))
            .ReturnsAsync(chaptersSample);

        // Act
        var result = await _service.GetUserMangasWithThreeLastChapterByUserId("1");

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByUserIdAsync("1"), Times.Once);
        _chapterRepository.Verify(repo => repo.GetThreeLastByMangaIdAndSourceListAsync(1, sourceListSample),
            Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task DeleteUserMangasByMangaId_ShouldCallRepositoryDeleteMethod()
    {
        // Act
        await _service.DeleteUserMangasByMangaId(1, "1");

        // Assert
        _userMangaRepository.Verify(repo => repo.DeleteAsync(1, "1"), Times.Once());
    }

    [Fact]
    public async Task DeleteUserMangaByMangaIdAndSourceId_ShouldCallRepositoryDeleteMethod()
    {
        // Act
        await _service.DeleteUserMangaByMangaIdAndSourceId(1, 1, "1");

        // Assert
        _userMangaRepository.Verify(repo => repo.DeleteAsync(1, 1, "1"), Times.Once());
    }
}