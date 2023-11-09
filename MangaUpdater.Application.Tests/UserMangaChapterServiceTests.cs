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
    private readonly Mock<IUserChapterRepository> _userChapterRepository;
    private readonly Mock<IChapterRepository> _chapterRepository;
    private readonly Mock<IUserSourceService> _userSourceService;
    private readonly UserMangaChapterService _service;

    public UserMangaChapterServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _userMangaRepository = new Mock<IUserMangaRepository>();
        _userChapterRepository = new Mock<IUserChapterRepository>();
        _chapterRepository = new Mock<IChapterRepository>();
        _userSourceService = new Mock<IUserSourceService>();
        _service = new UserMangaChapterService(_userMangaRepository.Object, _chapterRepository.Object,
            _userSourceService.Object, mapper, _userChapterRepository.Object);
    }

    [Fact]
    public async Task AddUserMangaBySourceIdList_Should_Add_Three_UserMangas()
    {
        // Arrange
        const string userId = "testUser";
        const int mangaId = 1;

        var sampleChapter1 = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Now, Number = "1" };
        var sampleChapter2 = new Chapter { Id = 2, MangaId = 1, SourceId = 2, Date = DateTime.Now, Number = "1" };
        var sampleChapter3 = new Chapter { Id = 3, MangaId = 1, SourceId = 3, Date = DateTime.Now, Number = "1" };
        var sampleUserSourceList = new List<UserSourceDto>
        {
            new(1, "Source1", false),
            new(2, "Source2", false),
            new(3, "Source3", false),
            new(4, "Source4", true)
        };
        var sourceList = new List<int> { 1, 2, 3, 4 };
        var sampleUserManga = new UserManga { Id = 1, MangaId = mangaId, UserId = userId };

        _userMangaRepository
            .Setup(service => service.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);

        _userSourceService
            .Setup(service => service.GetUserSourcesByMangaId(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleUserSourceList);

        // Act
        await _service.AddUserMangaBySourceIdList(It.IsAny<int>(), It.IsAny<string>(), sourceList);

        // Assert
        _userMangaRepository.Verify(service => service.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        _userSourceService.Verify(service => service.GetUserSourcesByMangaId(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once);
        _userChapterRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task AddUserMangaBySourceIdList_Should_Not_Add_Any_UserManga()
    {
        // Arrange
        const string userId = "testUser";
        const int mangaId = 1;
        
        var sampleUserSourceList = Enumerable.Empty<UserSourceDto>();
        var sourceList = new List<int> { 1 };
        var sampleUserManga = new UserManga { Id = 1, MangaId = mangaId, UserId = userId };

        _userMangaRepository
            .Setup(service => service.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);
        
        _userSourceService
            .Setup(service => service.GetUserSourcesByMangaId(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleUserSourceList);

        // Act
        await _service.AddUserMangaBySourceIdList(It.IsAny<int>(), It.IsAny<string>(), sourceList);

        // Assert
        _userSourceService.Verify(service => service.GetUserSourcesByMangaId(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once);
        _chapterRepository.VerifyNoOtherCalls();
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
                Manga = new Manga
                {
                    Id = 1,
                    CoverUrl = "url",
                    Synopsis = "",
                    Type = "Manga",
                    MyAnimeListId = 1,
                    MangaTitles = new List<MangaTitle> { new() { Id = 1, MangaId = 1, Name = "Manga1" } }
                },
                UserChapter = new UserChapter { UserMangaId = 1, SourceId = 1, ChapterId = 2, Source = baseSource, }
            },
        };
        var chaptersSample = new List<Chapter>
        {
            new()
            {
                Id = 1,
                MangaId = 1,
                SourceId = 1,
                Date = date,
                Number = "1",
                Source = baseSource
            },
            new()
            {
                Id = 2,
                MangaId = 1,
                SourceId = 1,
                Date = date,
                Number = "2",
                Source = baseSource
            },
            new()
            {
                Id = 3,
                MangaId = 1,
                SourceId = 1,
                Date = date,
                Number = "3",
                Source = baseSource
            }
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
                    new()
                    {
                        ChapterId = 1, SourceId = 1, SourceName = "Source1", Date = date, Number = "1", Read = true, IsUserAllowedToRead = true
                    },
                    new()
                    {
                        ChapterId = 2, SourceId = 1, SourceName = "Source1", Date = date, Number = "2", Read = true, IsUserAllowedToRead = true
                    },
                    new()
                    {
                        ChapterId = 3, SourceId = 1, SourceName = "Source1", Date = date, Number = "3", Read = false, IsUserAllowedToRead = true
                    },
                }
            }
        };

        _userMangaRepository
            .Setup(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(userMangaSample);
        _chapterRepository
            .Setup(repo => repo.GetThreeLastByMangaIdAndSourceListAsync(It.IsAny<int>(), sourceListSample))
            .ReturnsAsync(chaptersSample);

        // Act
        var result = await _service.GetUserMangasWithThreeLastChapterByUserId(It.IsAny<string>());

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        _chapterRepository.Verify(
            repo => repo.GetThreeLastByMangaIdAndSourceListAsync(It.IsAny<int>(), sourceListSample),
            Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task DeleteUserMangasByMangaId_Should_Call_Repository_Delete_Method()
    {
        // Act
        await _service.DeleteUserMangasByMangaId(It.IsAny<int>(), It.IsAny<string>());

        // Assert
        _userMangaRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async Task DeleteUserMangaByMangaIdAndSourceId_ShouldCallRepositoryDeleteMethod()
    {
        // Arrange
        var sampleUserManga = new UserManga { Id = 1, MangaId = 1, UserId = "user1" };

        _userMangaRepository
            .Setup(service => service.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);
        
        // Act
        await _service.DeleteUserMangaByMangaIdAndSourceId(1, It.IsAny<int>(), "user1");

        // Assert
        _userChapterRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once());
    }
}