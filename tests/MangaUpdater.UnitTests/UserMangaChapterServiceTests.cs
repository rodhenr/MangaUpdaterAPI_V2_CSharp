using AutoMapper;
using MangaUpdater.Core.Dtos;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
using MangaUpdater.Core.Common.Exceptions;
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
        _userMangaRepository = new Mock<IUserMangaRepository>();
        _userChapterRepository = new Mock<IUserChapterRepository>();
        _chapterRepository = new Mock<IChapterRepository>();
        _userSourceService = new Mock<IUserSourceService>();
        _service = new UserMangaChapterService(_userMangaRepository.Object, _chapterRepository.Object,
            _userSourceService.Object, _userChapterRepository.Object);
    }

    [Fact]
    public async Task AddUserMangaBySourceIdList_Should_Add_Three_UserMangas()
    {
        // Arrange
        const string userId = "testUser";
        const int mangaId = 1;

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
                UserChapter = new List<UserChapter>
                    { new() { UserMangaId = 1, SourceId = 1, ChapterId = 2, Source = baseSource } }
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
                        ChapterId = 1, SourceId = 1, SourceName = "Source1", Date = date, Number = "1", Read = true,
                        IsUserAllowedToRead = true
                    },
                    new()
                    {
                        ChapterId = 2, SourceId = 1, SourceName = "Source1", Date = date, Number = "2", Read = true,
                        IsUserAllowedToRead = true
                    },
                    new()
                    {
                        ChapterId = 3, SourceId = 1, SourceName = "Source1", Date = date, Number = "3", Read = false,
                        IsUserAllowedToRead = true
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
        var result =
            await _service.GetUserMangasWithThreeLastChapterByUserId(It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>());

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        _chapterRepository.Verify(
            repo => repo.GetThreeLastByMangaIdAndSourceListAsync(It.IsAny<int>(), sourceListSample),
            Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task DeleteUserMangasByMangaId_Should_Call_Repository_Delete_Method_Using_UserMangaId()
    {
        // Arrange
        const int mangaId = 1;
        const string userId = "user1";
        const int chapterId = 1;
        const int sourceId = 1;
        var sampleUserManga = new UserManga
        {
            Id = 1,
            MangaId = mangaId,
            UserId = userId,
            Manga = new Manga
            {
                Id = mangaId,
                CoverUrl = "cover",
                Synopsis = "",
                Type = "Manga",
                MyAnimeListId = 1,
                MangaTitles = Enumerable.Empty<MangaTitle>()
            }
        };

        _userMangaRepository
            .Setup(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);

        // Act
        await _service.DeleteUserMangasByMangaId(It.IsAny<int>(), It.IsAny<string>());

        // Assert
        _userChapterRepository.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Once());
    }

    [Fact]
    public async Task DeleteUserMangasByMangaId_Should_Call_Repository_Delete_Method_Using_()
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

    [Fact]
    public async Task UpdateOrCreateUserChapter_Should_Throw_BadRequestException_When_UserManga_Is_Null()
    {
        // Arrange
        UserManga? sampleUserManga = null;

        _userMangaRepository
            .Setup(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.UpdateOrCreateUserChapter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
        _userMangaRepository.Verify(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task UpdateOrCreateUserChapter_Should_Throw_BadRequestException_When_Chapter_Is_Null()
    {
        // Arrange
        const int mangaId = 1;
        const string userId = "user1";
        const int chapterId = 1;
        const int sourceId = 1;
        var sampleUserManga = new UserManga
        {
            Id = 1,
            MangaId = mangaId,
            UserId = userId,
            Manga = new Manga
            {
                Id = mangaId,
                CoverUrl = "cover",
                Synopsis = "",
                Type = "Manga",
                MyAnimeListId = 1,
                MangaTitles = Enumerable.Empty<MangaTitle>()
            }
        };
        Chapter? sampleChapter = null;

        _userMangaRepository
            .Setup(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);

        _chapterRepository
            .Setup(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleChapter);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() =>
            _service.UpdateOrCreateUserChapter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
        _userMangaRepository.Verify(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        _chapterRepository.Verify(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateOrCreateUserChapter_Should_Update_UserChapter_And_SaveChanges()
    {
        // Arrange
        const int mangaId = 1;
        const string userId = "user1";
        const int chapterId = 1;
        const int sourceId = 1;
        var sampleUserManga = new UserManga
        {
            Id = 1,
            MangaId = mangaId,
            UserId = userId,
            Manga = new Manga
            {
                Id = mangaId,
                CoverUrl = "cover",
                Synopsis = "",
                Type = "Manga",
                MyAnimeListId = 1,
                MangaTitles = Enumerable.Empty<MangaTitle>()
            }
        };
        var sampleChapter = new Chapter
        {
            Id = chapterId,
            MangaId = mangaId,
            SourceId = sourceId,
            Date = new DateTime(),
            Number = "1"
        };
        var sampleUserChapter = new UserChapter
        {
            Id = 1,
            SourceId = sourceId,
            UserMangaId = 1,
            ChapterId = chapterId
        };

        _userMangaRepository
            .Setup(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleUserManga);

        _chapterRepository
            .Setup(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleChapter);

        _userChapterRepository
            .Setup(repo => repo.GetByUserMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(sampleUserChapter);

        // Act
        await _service.UpdateOrCreateUserChapter(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

        // Assert
        _userMangaRepository.Verify(repo => repo.GetByMangaIdAndUserIdAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        _chapterRepository.Verify(repo => repo.GetByIdAndMangaIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        _userChapterRepository.Verify(repo => repo.GetByUserMangaIdAndSourceIdAsync(It.IsAny<int>(), It.IsAny<int>()),
            Times.Once);
        _userChapterRepository.Verify(repo => repo.SaveAsync(), Times.Once);
    }
}