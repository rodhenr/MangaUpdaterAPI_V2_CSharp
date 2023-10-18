using System.Security.Claims;
using MangaUpdater.API.Controllers;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaUpdater.API.Tests;

public class MangaControllerTests
{
    private readonly MangaController _mangaController;
    private readonly Mock<IMangaService> _mangaService;
    private readonly Mock<IUserSourceService> _userSourceService;
    private readonly Mock<IRegisterMangaFromMyAnimeListService> _registerMangaFromMyAnimeListService;
    private readonly Mock<IMangaLivreService> _mangaLivreService;
    private readonly Mock<IMangaSourceService> _mangaSourceService;
    private readonly Mock<IChapterService> _chapterService;

    public MangaControllerTests()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "testUser") };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

        _mangaService = new Mock<IMangaService>();
        _userSourceService = new Mock<IUserSourceService>();
        _registerMangaFromMyAnimeListService = new Mock<IRegisterMangaFromMyAnimeListService>();
        _mangaLivreService = new Mock<IMangaLivreService>();
        _mangaSourceService = new Mock<IMangaSourceService>();
        _chapterService = new Mock<IChapterService>();


        _mangaController = new MangaController(_mangaService.Object, _userSourceService.Object,
            _registerMangaFromMyAnimeListService.Object, _mangaLivreService.Object, _mangaSourceService.Object,
            _chapterService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            }
        };
    }

    [Fact]
    public async Task GetMangas_Should_Returns_Ok_Result()
    {
        // Arrange
        var mangaUserDtoList = new List<MangaUserDto>
        {
            new(1, "Cover1", "Manga1"),
            new(2, "Cover2", "Manga2"),
            new(3, "Cover3", "Manga3")
        };
        _mangaService
            .Setup(service => service.GetWithFilter(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>()))
            .ReturnsAsync(mangaUserDtoList);

        // Act
        var result = await _mangaController.GetMangas();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<MangaUserDto>>(okResult.Value);
    }

    [Fact]
    public async Task RegisterManga_Should_Returns_Ok_Result()
    {
        // Arrange
        var sampleManga = new Manga { Id = 1, CoverUrl = "url", Synopsis = "", Type = "Manga", MyAnimeListId = 1 };
        _registerMangaFromMyAnimeListService
            .Setup(service => service.RegisterMangaFromMyAnimeListById(It.IsAny<int>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _mangaController.RegisterManga(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<Manga>(okResult.Value);
    }

    [Fact]
    public async Task GetManga_WithAuthenticatedUser_ReturnsOkResult()
    {
        // Arrange
        const int mangaId = 123;
        const string userId = "testUser";
        var sampleMangaDto = new MangaDto
        {
            CoverUrl = "url",
            Name = "Manga1",
            AlternativeName = "",
            Author = "Author1",
            Synopsis = "",
            Type = "Manga",
            MyAnimeListId = 1,
            IsUserFollowing = true,
            Sources = Enumerable.Empty<SourceDto>(),
            Genres = Enumerable.Empty<string>(),
            Chapters = Enumerable.Empty<ChapterDto>(),
        };

        _mangaService
            .Setup(service => service.GetByIdAndUserId(mangaId, userId))
            .ReturnsAsync(sampleMangaDto);

        // Act
        var result = await _mangaController.GetManga(mangaId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<MangaDto>(okResult.Value);
    }

    [Fact]
    public async Task GetManga_WithUnauthenticatedUser_ReturnsOkResult()
    {
        // Arrange
        const int mangaId = 123;
        var sampleMangaDto = new MangaDto
        {
            CoverUrl = "url",
            Name = "Manga1",
            AlternativeName = "",
            Author = "Author1",
            Synopsis = "",
            Type = "Manga",
            MyAnimeListId = 1,
            IsUserFollowing = true,
            Sources = Enumerable.Empty<SourceDto>(),
            Genres = Enumerable.Empty<string>(),
            Chapters = Enumerable.Empty<ChapterDto>(),
        };

        _mangaController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = null! }
        };

        _mangaService
            .Setup(service => service.GetByIdNotLogged(mangaId))
            .ReturnsAsync(sampleMangaDto);

        // Act
        var result = await _mangaController.GetManga(mangaId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<MangaDto>(okResult.Value);
    }

    [Fact]
    public async Task GetUserSources_WithAuthenticatedUser_ReturnsOkResult()
    {
        // Arrange
        const int mangaId = 123;
        const string userId = "testUser";
        var userSourceDtoList = new List<UserSourceDto>
        {
            new(1, "Source1", true),
            new(2, "Source2", true),
            new(3, "Source3", false),
        };

        _userSourceService
            .Setup(service => service.GetUserSourcesByMangaId(mangaId, userId))
            .ReturnsAsync(userSourceDtoList);

        // Act
        var result = await _mangaController.GetUserSources(mangaId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsAssignableFrom<IEnumerable<UserSourceDto>>(okResult.Value);
    }

    [Fact]
    public async Task AddSourceToMangaAndGetData_ValidSourceId_ReturnsOkResult()
    {
        // Arrange
        const int mangaId = 123;
        const int sourceId = 1;
        const string mangaUrl = "https://example.com/manga-url";

        _mangaLivreService
            .Setup(service => service.RegisterSourceAndChapters(mangaId, sourceId, mangaUrl))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _mangaController.AddSourceToMangaAndGetData(mangaId, sourceId, mangaUrl);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateChaptersFromSource_ValidSourceId_ReturnsOkResult()
    {
        // Arrange
        const int mangaId = 123;
        const int sourceId = 1;
        const string lastChapterId = "456";
        const string mangaSourceUrl = "https://example.com/manga-source-url";
        var sampleMangaSource = new MangaSource { Id = 1, MangaId = 1, SourceId = 1, Url = "" };
        var sampleChapter = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Number = "1", Date = DateTime.Now };
        
        _mangaSourceService
            .Setup(service => service.GetByMangaIdAndSourceId(mangaId, sourceId))
            .ReturnsAsync(sampleMangaSource);
        _chapterService
            .Setup(service => service.GetLastByMangaIdAndSourceId(mangaId, sourceId))
            .ReturnsAsync(sampleChapter);
        _mangaLivreService
            .Setup(service => service.UpdateChapters(mangaId, sourceId, lastChapterId, mangaSourceUrl))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _mangaController.UpdateChaptersFromSource(mangaId, sourceId);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}