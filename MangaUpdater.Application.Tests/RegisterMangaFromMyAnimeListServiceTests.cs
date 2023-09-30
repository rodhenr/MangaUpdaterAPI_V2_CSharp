using AutoMapper;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Models.External.MyAnimeList;
using MangaUpdater.Application.Services.External.MyAnimeList;
using MangaUpdater.Domain.Entities;

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
    public async Task RegisterMangaFromMyAnimeListById_ShouldRegisterManga()
    {
        // Arrange
        const int malMangaId = 1;
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

        _mangaService.Setup(service => service.CheckIfMangaIsRegistered(malMangaId)).ReturnsAsync(false);
        _malApiService.Setup(service => service.GetMangaFromMyAnimeListByIdAsync(malMangaId))
            .ReturnsAsync(sampleMyAnimeListApiResponse);

        // Act
        var result = await _service.RegisterMangaFromMyAnimeListById(1);

        // Assert
        _mangaService.Verify(service => service.CheckIfMangaIsRegistered(malMangaId), Times.Once);
        _malApiService.Verify(service => service.GetMangaFromMyAnimeListByIdAsync(malMangaId), Times.Once);
        _mangaService.Verify(service => service.Add(It.IsAny<Manga>()), Times.Once);

        result.Should().BeEquivalentTo(expectedManga);
    }
}