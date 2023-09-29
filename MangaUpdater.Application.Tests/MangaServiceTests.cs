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
        // Arrange
        var manga = new Manga { Id = 1, CoverUrl = "", Synopsis = "", Type = "Manga", MyAnimeListId = 1 };

        // Act
        await _service.Add(manga);

        // Assert
        _repository.Verify(repo => repo.Create(manga), Times.Once);
    }

    [Fact]
    public async Task CheckIfMangaIsRegistered_Should_Return_True_When_Manga_Exists()
    {
        // Arrange
        const int myAnimeListId = 1;
        var manga = new Manga { Id = 1, CoverUrl = "", Synopsis = "", Type = "Manga", MyAnimeListId = 1 };
        _repository
            .Setup(repo => repo.GetByMalIdAsync(myAnimeListId))
            .ReturnsAsync(manga);

        // Act
        var result = await _service.CheckIfMangaIsRegistered(myAnimeListId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CheckIfMangaIsRegistered_Should_Return_False_When_Manga_Does_Not_Exists()
    {
        // Arrange
        const int myAnimeListId = 1;
        _repository
            .Setup(repo => repo.GetByMalIdAsync(myAnimeListId))
            .ReturnsAsync((Manga)null);

        // Act
        var result = await _service.CheckIfMangaIsRegistered(myAnimeListId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetWithFilter_Should_Return_MangaUserDtos_With_Valid_Parameters()
    {
        // Arrange
        const int page = 1;
        const string orderBy = "Name";
        var sourceIdList = new List<int> { 1, 2 };
        var genreIdList = new List<int> { 3, 4 };
        var sampleMangas = new List<Manga>
        {
            new()
            {
                Id = 1,
                CoverUrl = "cover1.jpg",
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
                CoverUrl = "cover2.jpg",
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
                CoverUrl = "cover1.jpg",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 3, Name = "Manga3" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 3
            },
            new()
            {
                Id = 4,
                CoverUrl = "cover2.jpg",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 4, Name = "Manga4" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 4
            },
            new()
            {
                Id = 5,
                CoverUrl = "cover1.jpg",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 5, Name = "Manga5" }
                },
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 5
            },
            new()
            {
                Id = 6,
                CoverUrl = "cover2.jpg",
                MangaTitles = new List<MangaTitle>()
                {
                    new() { MangaId = 6, Name = "Manga6" }
                },
                Synopsis = "",
                Type = "Manga",
                MyAnimeListId = 6
            }
        };
        _repository
            .Setup(repo => repo.GetWithFiltersAsync(page, orderBy, sourceIdList, genreIdList))
            .ReturnsAsync(sampleMangas);

        // Act
        var result = await _service.GetWithFilter(page, orderBy, sourceIdList, genreIdList);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<MangaUserDto>>(result);

        var resultList = result.ToList();

        Assert.Equal(sampleMangas.Count, resultList.Count);
        Assert.Equal(sampleMangas[0].Id, resultList[0].Id);
        Assert.Equal(sampleMangas[0].CoverUrl, resultList[0].CoverUrl);
        Assert.Equal(sampleMangas[0].MangaTitles!.First().Name, resultList[0].Name);
    }

    [Fact]
    public async Task GetWithFilter_Should_Return_Empty_List_When_No_Mangas_Found()
    {
        // Arrange
        const int page = 1;
        const string orderBy = "Name";
        var sourceIdList = new List<int> { 1, 2 };
        var genreIdList = new List<int> { 3, 4 };
        var sampleMangas = new List<Manga>();
        _repository
            .Setup(repo => repo.GetWithFiltersAsync(page, orderBy, sourceIdList, genreIdList))
            .ReturnsAsync(sampleMangas);

        // Act
        var result = await _service.GetWithFilter(page, orderBy, sourceIdList, genreIdList);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdNotLogged_Should_Return_MangaDto_With_Valid_Id()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleManga = new Manga
        {
            CoverUrl = "url",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            Chapters = new List<Chapter>()
            {
                new()
                {
                    Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = 1,
                    Source = new Source() { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            },
            MangaTitles = new List<MangaTitle>()
            {
                new() { Id = 1, MangaId = 1, Name = "Manga1", },
                new() { Id = 1, MangaId = 1, Name = "AltManga1" }
            },
            MangaAuthors = new List<MangaAuthor>()
            {
                new() { Id = 1, MangaId = 1, Name = "Author1" }
            },
            UserMangas = new List<UserManga>()
            {
                new() { Id = 1, UserId = "1", MangaId = 1, SourceId = 1, CurrentChapterId = 1 }
            },
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
                    Number = 1,
                    Read = true
                }
            }
        };
        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(1))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdNotLogged(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MangaDto>(result);
        _repository.Verify(repo => repo.GetByIdOrderedDescAsync(1), Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task GetByIdNotLogged_Should_Throw_Exception_When_Manga_Is_Null()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(1))
            .ReturnsAsync((Manga)null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetByIdNotLogged(1));
    }

    [Fact]
    public async Task GetByIdAndUserId_Should_Return_MangaDto_With_Valid_Id()
    {
        // Arrange
        var date = DateTime.Now;
        var sampleManga = new Manga
        {
            CoverUrl = "url",
            Synopsis = "synopsis",
            Type = "Manga",
            MyAnimeListId = 1,
            Chapters = new List<Chapter>()
            {
                new()
                {
                    Id = 1, MangaId = 1, SourceId = 1, Date = date, Number = 1,
                    Source = new Source() { Id = 1, Name = "Source1", BaseUrl = "base" }
                }
            },
            MangaTitles = new List<MangaTitle>()
            {
                new() { Id = 1, MangaId = 1, Name = "Manga1", },
                new() { Id = 1, MangaId = 1, Name = "AltManga1" }
            },
            MangaAuthors = new List<MangaAuthor>()
            {
                new() { Id = 1, MangaId = 1, Name = "Author1" }
            },
            UserMangas = new List<UserManga>()
            {
                new() { Id = 1, UserId = "1", MangaId = 1, SourceId = 1, CurrentChapterId = 1 }
            },
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
                    Number = 1,
                    Read = true
                }
            }
        };
        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(1, "1"))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdAndUserId(1, "1");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MangaDto>(result);
        _repository.Verify(repo => repo.GetByIdAndUserIdOrderedDescAsync(1, "1"), Times.Once);
        result.Should().BeEquivalentTo(expectedMangaDto);
    }

    [Fact]
    public async Task GetByIdAndUserId_Should_Throw_Exception_When_Manga_Is_Null()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(1, "1"))
            .ReturnsAsync((Manga)null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.GetByIdAndUserId(1, "1"));
    }
}