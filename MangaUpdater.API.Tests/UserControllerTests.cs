using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MangaUpdater.API.Controllers;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Models.Login;
using MangaUpdater.Application.Models.Register;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.API.Tests;

public class UserControllerTests
{
    private readonly UserController _userController;
    private readonly Mock<IUserMangaChapterService> _userMangaChapterService;
    private readonly Mock<IChapterService> _chapterService;
    private readonly Mock<IUserMangaService> _userMangaService;

    public UserControllerTests()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "testUser") };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

        _userMangaChapterService = new Mock<IUserMangaChapterService>();
        _chapterService = new Mock<IChapterService>();
        _userMangaService = new Mock<IUserMangaService>();

        _userController = new UserController(_userMangaChapterService.Object, _chapterService.Object,
            _userMangaService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            }
        };
    }

    [Fact]
    public async Task GetLoggedUserMangas_Should_Returns_Ok_Result()
    {
        // Arrange
        var mangaUserDtoList = new List<MangaUserLoggedDto>
        {
            new() { Id = 1, Name = "Manga1", CoverUrl = "", Chapters = Enumerable.Empty<ChapterDto>() },
            new() { Id = 2, Name = "Manga2", CoverUrl = "", Chapters = Enumerable.Empty<ChapterDto>() },
            new() { Id = 3, Name = "Manga3", CoverUrl = "", Chapters = Enumerable.Empty<ChapterDto>() },
            new() { Id = 4, Name = "Manga4", CoverUrl = "", Chapters = Enumerable.Empty<ChapterDto>() }
        };

        _userMangaChapterService
            .Setup(s => s.GetUserMangasWithThreeLastChapterByUserId("testUser"))
            .ReturnsAsync(mangaUserDtoList);

        // Act
        var result = await _userController.GetLoggedUserMangas();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<MangaUserLoggedDto>>(okResult.Value);
        response.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetUserMangasList_Should_Returns_Ok_Result()
    {
        // Arrange
        var mangaUserDtoList = new List<MangaUserDto>
        {
            new(1, "Manga1", ""),
            new(2, "Manga2", ""),
            new(3, "Manga3", ""),
            new(4, "Manga4", "")
        };

        _userMangaService
            .Setup(s => s.GetMangasByUserId("testUser"))
            .ReturnsAsync(mangaUserDtoList);

        // Act
        var result = await _userController.GetUserMangasList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<MangaUserDto>>(okResult.Value);
        response.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetUserMangas_Should_Returns_Ok_Result()
    {
        // Arrange
        var mangaUserDtoList = new List<MangaUserDto>
        {
            new(1, "Manga1", ""),
            new(2, "Manga2", ""),
            new(3, "Manga3", ""),
            new(4, "Manga4", "")
        };

        _userMangaService
            .Setup(s => s.GetMangasByUserId("testUser"))
            .ReturnsAsync(mangaUserDtoList);

        // Act
        var result = await _userController.GetUserMangas("testUser");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<List<MangaUserDto>>(okResult.Value);
        response.Should().HaveCount(4);
    }

    [Fact]
    public async Task FollowSourcesFromManga_Should_Returns_Ok_Result()
    {
        // Arrange
        const int mangaId = 1;
        var selectedSources = new List<int> { 1, 2 };

        _userMangaChapterService
            .Setup(s => s.AddUserMangaBySourceIdList(mangaId, "testUser", selectedSources))
            .Verifiable();

        // Act
        var result = await _userController.FollowSourcesFromManga(mangaId, selectedSources);

        // Assert
        _userMangaChapterService.Verify(
            service => service.AddUserMangaBySourceIdList(mangaId, "testUser", selectedSources), Times.Once);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UnfollowManga_Should_Returns_Ok_Result()
    {
        // Arrange
        const int mangaId = 1;

        _userMangaChapterService
            .Setup(s => s.DeleteUserMangasByMangaId(mangaId, "testUser"))
            .Verifiable();

        // Act
        var result = await _userController.UnfollowManga(mangaId);

        // Assert
        _userMangaChapterService.Verify(service => service.DeleteUserMangasByMangaId(mangaId, "testUser"), Times.Once);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteUserManga_Should_Returns_Ok_Result()
    {
        // Arrange
        const int mangaId = 1;
        const int sourceId = 1;

        _userMangaChapterService
            .Setup(s => s.DeleteUserMangaByMangaIdAndSourceId(mangaId, sourceId, "testUser"))
            .Verifiable();

        // Act
        var result = await _userController.DeleteUserManga(mangaId, sourceId);

        // Assert
        _userMangaChapterService.Verify(
            service => service.DeleteUserMangaByMangaIdAndSourceId(mangaId, sourceId, "testUser"), Times.Once);

        Assert.IsType<OkResult>(result);
    }

    // [Fact]
    // public async Task UpdateManga_Returns_Ok_Result()
    // {
    //     // Arrange
    //     const int mangaId = 1;
    //     const int sourceId = 1;
    //     const int chapterId = 456;
    //     var sampleUserManga = new UserManga
    //         { Id = 1, MangaId = 1, SourceId = 1, UserId = "testUser" };
    //
    //     _userMangaService
    //         .Setup(service => service.GetByMangaIdUserIdAndSourceId(mangaId, "testUser", sourceId))
    //         .ReturnsAsync(sampleUserManga);
    //
    //     _userMangaService
    //         .Setup(s => s.Update(sampleUserManga))
    //         .Verifiable();
    //
    //     // Act
    //     var result = await _userController.UpdateManga(mangaId, sourceId, chapterId);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkResult>(result);
    //     _userMangaService.Verify(service => service.GetByMangaIdUserIdAndSourceId(mangaId, "testUser", sourceId),
    //         Times.Once);
    //     _userMangaService.Verify(service => service.Update(It.Is<UserManga>(um => um.CurrentChapterId == chapterId)),
    //         Times.Once);
    // }
}