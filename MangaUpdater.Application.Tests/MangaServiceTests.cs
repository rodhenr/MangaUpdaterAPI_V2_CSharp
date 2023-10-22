using AutoMapper;
using MangaUpdater.Application.DTOs;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Domain.Interfaces;

namespace MangaUpdater.Application.Tests;

public class MangaServiceTests
{
    private readonly Mock<IMangaRepository> _repository;
    private readonly MangaService _service;

    public MangaServiceTests()
    {
        var profile = new MappingProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        var mapper = new Mapper(configuration);

        _repository = new Mock<IMangaRepository>();
        _service = new MangaService(_repository.Object, mapper);
    }

    [Fact]
    public async Task Add_Should_Call_Create_Method_In_Repository()
    {
        // Act
        await _service.Add(It.IsAny<Manga>());

        // Assert
        _repository.Verify(repo => repo.Create(It.IsAny<Manga>()), Times.Once);
    }

    [Fact]
    public async Task CheckIfMangaIsRegistered_Should_Return_True_When_Manga_Exists()
    {
        // Arrange
        var manga = new Manga { Id = 1, CoverUrl = "", Synopsis = "", Type = "Manga", MyAnimeListId = 1 };
        _repository
            .Setup(repo => repo.GetByMalIdAsync(It.IsAny<int>()))
            .ReturnsAsync(manga);

        // Act
        var result = await _service.CheckIfMangaIsRegistered(It.IsAny<int>());

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckIfMangaIsRegistered_Should_Return_False_When_Manga_Does_Not_Exists()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByMalIdAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        // Act
        var result = await _service.CheckIfMangaIsRegistered(It.IsAny<int>());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetWithFilter_Should_Return_MangaUserDtos_With_Valid_Parameters()
    {
        // Arrange
        var sampleMangas = new List<Manga>
        {
            new()
            {
                Id = 1,
                CoverUrl = "cover1",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 1, Name = "Manga1" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 1
            },
            new()
            {
                Id = 2,
                CoverUrl = "cover2",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 2, Name = "Manga2" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 2
            },
            new()
            {
                Id = 3,
                CoverUrl = "cover3",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 3, Name = "Manga3" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 3
            }
        };
        var expectedDto = new List<MangaUserDto>
        {
            new(1, "cover1", "Manga1"),
            new(2, "cover2", "Manga2"),
            new(3, "cover3", "Manga3")
        };

        _repository
            .Setup(repo => repo.GetWithFiltersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>()))
            .ReturnsAsync(sampleMangas);

        // Act
        var result = await _service.GetWithFilter(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
            It.IsAny<List<int>>());

        // Assert
        _repository.Verify(
            repo => repo.GetWithFiltersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>()), Times.Once);
        result.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task GetWithFilter_Should_Return_Empty_List_When_No_Mangas_Found()
    {
        // Arrange
        var sampleMangas = Enumerable.Empty<Manga>();

        _repository
            .Setup(repo => repo.GetWithFiltersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>()))
            .ReturnsAsync(sampleMangas);

        // Act
        var result = await _service.GetWithFilter(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
            It.IsAny<List<int>>());

        // Assert
        _repository.Verify(
            repo => repo.GetWithFiltersAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>()), Times.Once);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdNotLogged_Should_Return_MangaDto_With_Valid_Id()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleManga = new Manga
        {
            Id = 1,
            CoverUrl = "url",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            Chapters = new List<Chapter>()
            {
                new()
                {
                    Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = "1",
                    Source = new Source() { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            },
            MangaTitles = new List<MangaTitle>()
            {
                new() { Id = 1, MangaId = 1, Name = "Manga1", },
                new() { Id = 1, MangaId = 1, Name = "AltManga1" }
            },
            MangaAuthors = new List<MangaAuthor>
            {
                new() { Id = 1, MangaId = 1, Name = "Author1" }
            },
            UserMangas = new List<UserManga>
            {
                new()
                {
                    Id = 1,
                    UserId = "1",
                    MangaId = 1,
                    UserChapter = new UserChapter { UserMangaId = 1, SourceId = 1, ChapterId = 1 }
                }
            },
            MangaGenres = new List<MangaGenre>
            {
                new()
                {
                    Id = 1,
                    MangaId = 1,
                    GenreId = 1,
                    Genre = new Genre() { Id = 1, Name = "Genre1" }
                }
            },
            MangaSources = new List<MangaSource>
            {
                new()
                {
                    Id = 1,
                    MangaId = 1,
                    SourceId = 1,
                    Url = "http",
                    Source = new Source { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            }
        };
        var expectedMangaDto = new MangaDto
        {
            MangaId = 1,
            CoverUrl = "url",
            Name = "Manga1",
            AlternativeName = "AltManga1",
            Author = "Author1",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            IsUserFollowing = true,
            Sources = new List<SourceDto>()
            {
                new(1, "Source1", "base")
            },
            Genres = new List<string>()
            {
                "Genre1"
            },
            Chapters = new List<ChapterDto>()
            {
                new()
                {
                    ChapterId = 1,
                    SourceId = 1,
                    SourceName = "Source1",
                    Date = date,
                    Number = "1",
                    Read = true
                }
            }
        };
        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdNotLogged(It.IsAny<int>());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MangaDto>(result);
        _repository.Verify(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task GetByIdNotLogged_Should_Throw_Exception_When_Manga_Is_Null()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetByIdNotLogged(It.IsAny<int>()));
    }

    [Fact]
    public async Task GetByIdAndUserId_Should_Return_MangaDto_With_Valid_Id()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleManga = new Manga
        {
            Id = 1,
            CoverUrl = "url",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            Chapters = new List<Chapter>
            {
                new()
                {
                    Id = 1,
                    MangaId = 1,
                    SourceId = 1,
                    Date = date,
                    Number = "1",
                    Source = new Source { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            },
            MangaTitles = new List<MangaTitle>
            {
                new() { Id = 1, MangaId = 1, Name = "Manga1", },
                new() { Id = 1, MangaId = 1, Name = "AltManga1" }
            },
            MangaAuthors = new List<MangaAuthor>
            {
                new() { Id = 1, MangaId = 1, Name = "Author1" }
            },
            UserMangas = new List<UserManga>
            {
                new()
                {
                    Id = 1,
                    UserId = "1",
                    MangaId = 1,
                    UserChapter = new UserChapter { UserMangaId = 1, SourceId = 1, ChapterId = 1 }
                }
            },
            MangaGenres = new List<MangaGenre>
            {
                new()
                {
                    Id = 1,
                    MangaId = 1,
                    GenreId = 1,
                    Genre = new Genre { Id = 1, Name = "Genre1" }
                }
            },
            MangaSources = new List<MangaSource>
            {
                new()
                {
                    Id = 1, MangaId = 1, SourceId = 1, Url = "http",
                    Source = new Source { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            }
        };
        var expectedMangaDto = new MangaDto
        {
            MangaId = 1,
            CoverUrl = "url",
            Name = "Manga1",
            AlternativeName = "AltManga1",
            Author = "Author1",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            IsUserFollowing = true,
            Sources = new List<SourceDto>()
            {
                new(1, "Source1", "base")
            },
            Genres = new List<string>()
            {
                "Genre1"
            },
            Chapters = new List<ChapterDto>()
            {
                new()
                {
                    ChapterId = 1,
                    SourceId = 1,
                    SourceName = "Source1",
                    Date = date,
                    Number = "1",
                    Read = true
                }
            }
        };
        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>());

        // Assert
        _repository.Verify(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task GetByIdAndUserId_Should_Throw_Exception_When_Manga_Is_Null()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(() => null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task GetByIdAndUserId_Should_Return_Empty_MangaDto()
    {
        // Arrange
        var sampleManga = new Manga
        {
            Id = 1,
            CoverUrl = "url",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            Chapters = null,
            MangaTitles = new List<MangaTitle>()
            {
                new() { Id = 1, MangaId = 1, Name = "Manga1", },
                new() { Id = 1, MangaId = 1, Name = "AltManga1" }
            },
            MangaAuthors = new List<MangaAuthor>()
            {
                new() { Id = 1, MangaId = 1, Name = "Author1" }
            },
            UserMangas = Enumerable.Empty<UserManga>(),
            MangaGenres = new List<MangaGenre>()
            {
                new() { Id = 1, MangaId = 1, GenreId = 1, Genre = new Genre() { Id = 1, Name = "Genre1" } }
            },
            MangaSources = new List<MangaSource>()
            {
                new()
                {
                    Id = 1, MangaId = 1, SourceId = 1, Url = "http",
                    Source = new Source() { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            }
        };
        var expectedMangaDto = new MangaDto
        {
            MangaId = 1,
            CoverUrl = "url",
            Name = "Manga1",
            AlternativeName = "AltManga1",
            Author = "Author1",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            IsUserFollowing = false,
            Sources = new List<SourceDto>()
            {
                new(1, "Source1", "base")
            },
            Genres = new List<string>()
            {
                "Genre1"
            },
            Chapters = Enumerable.Empty<ChapterDto>()
        };

        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>());

        // Assert
        _repository.Verify(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }
}