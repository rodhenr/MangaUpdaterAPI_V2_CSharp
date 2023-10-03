using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Models.External.MyAnimeList;
using MangaUpdater.Application.Services.External.MyAnimeList;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Application.Tests;

public class RegisterMangaFromMyAnimeListServiceTests
{
    private readonly Mock<IMyAnimeListApiService> _malApiService;
    private readonly Mock<IMangaService> _mangaService;
    private readonly Mock<IMangaGenreService> _mangaGenreService;
    private readonly Mock<IMangaAuthorService> _mangaAuthorService;
    private readonly Mock<IMangaTitleService> _mangaTitleService;
    private readonly RegisterMangaFromMyAnimeListService _service;

    public RegisterMangaFromMyAnimeListServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _malApiService = new Mock<IMyAnimeListApiService>();
        _mangaService = new Mock<IMangaService>();
        _mangaGenreService = new Mock<IMangaGenreService>();
        _mangaAuthorService = new Mock<IMangaAuthorService>();
        _mangaTitleService = new Mock<IMangaTitleService>();

        _service = new RegisterMangaFromMyAnimeListService(_malApiService.Object, mapper, _mangaService.Object,
            _mangaGenreService.Object, _mangaAuthorService.Object, _mangaTitleService.Object);
    }

    [Fact]
    public async Task RegisterMangaFromMyAnimeListById_Should_Throw_BadRequestException_If_Manga_Is_Already_Registered()
    {
        // Arrange
        _mangaService
            .Setup(s => s.CheckIfMangaIsRegistered(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act and Assert
        await Assert.ThrowsAsync<BadRequestException>(() => _service.RegisterMangaFromMyAnimeListById(It.IsAny<int>()));
    }

    [Fact]
    public async Task RegisterMangaFromMyAnimeListById_Should_Register_Manga()
    {
        // Arrange
        var sampleMyAnimeListApiResponse = new MyAnimeListApiResponse
        {
            MalId = 1,
            Titles = new List<TitleEntry> { new() { Title = "Title1", Type = "Title" } },
            Images = new ImagesSet { JPG = new Image { LargeImageUrl = "imageUrl" } },
            Type = "Manga",
            Status = "Publishing",
            Publishing = true,
            Synopsis = "abc",
            Genres = new List<MalCollection>
            {
                new() { MalId = 100, Type = "Genre", Url = "url1", Name = "Genre1" },
                new() { MalId = 101, Type = "Genre", Url = "url2", Name = "Genre2" },
            },
            Authors = new List<MalCollection>
            {
                new() { MalId = 102, Type = "Author", Url = "url1", Name = "Author1" }
            }
        };
        var expectedManga = new Manga
            { CoverUrl = "imageUrl", Synopsis = "abc", Type = "Manga", MyAnimeListId = 1 };

        _mangaService
            .Setup(service => service.CheckIfMangaIsRegistered(It.IsAny<int>()))
            .ReturnsAsync(false);
        _malApiService
            .Setup(service => service.GetMangaFromMyAnimeListByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(sampleMyAnimeListApiResponse);

        // Act
        var result = await _service.RegisterMangaFromMyAnimeListById(1);

        // Assert
        _mangaService.Verify(service => service.CheckIfMangaIsRegistered(It.IsAny<int>()), Times.Once);
        _malApiService.Verify(service => service.GetMangaFromMyAnimeListByIdAsync(It.IsAny<int>()), Times.Once);
        _mangaGenreService.Verify(mock => mock.BulkCreate(It.IsAny<IEnumerable<MangaGenre>>()), Times.Once);
        _mangaAuthorService.Verify(mock => mock.BulkCreate(It.IsAny<IEnumerable<MangaAuthor>>()), Times.Once);
        _mangaTitleService.Verify(mock => mock.BulkCreate(It.IsAny<IEnumerable<MangaTitle>>()), Times.Once);
        _mangaService.Verify(service => service.Add(It.IsAny<Manga>()), Times.Once);

        result.Should().BeEquivalentTo(expectedManga);
    }
}