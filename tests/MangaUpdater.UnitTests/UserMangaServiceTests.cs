using AutoMapper;
using MangaUpdater.Core.Dtos;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class UserMangaServiceTests
{
    private readonly Mock<IUserMangaRepository> _userMangaRepository;
    private readonly UserMangaService _service;

    public UserMangaServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _userMangaRepository = new Mock<IUserMangaRepository>();
        _service = new UserMangaService(mapper, _userMangaRepository.Object);
    }

    [Fact]
    public async Task GetMangasByUserId_Should_Return_MangaUserDtos()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new()
            {
                Id = 1,
                MangaId = 1,
                UserId = "1",
                Manga = new Manga
                {
                    Id = 1, CoverUrl = "url1", Synopsis = "", Type = "Manga", MyAnimeListId = 1,
                    MangaTitles = new List<MangaTitle> { new() { Id = 1, MangaId = 1, Name = "Manga1" } }
                }
            },
            new()
            {
                Id = 2,
                MangaId = 2,
                UserId = "1",
                Manga = new Manga
                {
                    Id = 2,
                    CoverUrl = "url2",
                    Synopsis = "",
                    Type = "Manga",
                    MyAnimeListId = 2,
                    MangaTitles = new List<MangaTitle>
                    {
                        new() { Id = 2, MangaId = 2, Name = "Manga2" }
                    }
                }
            },
            new()
            {
                Id = 3,
                MangaId = 3,
                UserId = "1",
                Manga = new Manga
                {
                    Id = 3,
                    CoverUrl = "url3",
                    Synopsis = "",
                    Type = "Manga",
                    MyAnimeListId = 3,
                    MangaTitles = new List<MangaTitle>
                    {
                        new() { Id = 3, MangaId = 3, Name = "Manga3" }
                    }
                }
            },
        };
        var expectedDto = new List<MangaUserDto>
        {
            new(1, "url1", "Manga1"),
            new(2, "url2", "Manga2"),
            new(3, "url3", "Manga3")
        };

        _userMangaRepository
            .Setup(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(sampleUserMangas);

        // Act
        var result = await _service.GetMangasByUserId(It.IsAny<string>());

        // Assert
        _userMangaRepository.Verify(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()), Times.Once);
        result.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task GetByUserIdAndMangaId_Should_Return_UserManga()
    {
        // Arrange
        const string userId = "user1";
        const int mangaId = 1;
        var userMangaSample = new UserManga { Id = 1, MangaId = 1, UserId = "1" };

        _userMangaRepository
            .Setup(repo => repo.GetByMangaIdAndUserIdAsync(mangaId, userId))
            .ReturnsAsync(userMangaSample);

        // Act
        var result = await _service.GetByUserIdAndMangaId(userId, mangaId);

        // Assert
        _userMangaRepository.Verify(repo => repo.GetByMangaIdAndUserIdAsync(mangaId, userId), Times.Once);
        result.Should().BeEquivalentTo(userMangaSample);
    }

    [Fact]
    public async Task Update_Should_Update_UserManga_And_SaveChanges()
    {
        // Act
        await _service.Update(It.IsAny<UserManga>());

        // Assert
        _userMangaRepository.Verify(repo => repo.Update(It.IsAny<UserManga>()), Times.Once);
        _userMangaRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<UserManga>()), Times.Once);
    }
}