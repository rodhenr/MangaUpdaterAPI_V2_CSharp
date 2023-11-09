using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class MangaRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IMangaRepository _repository;

    public MangaRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new MangaRepository(_context);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Manga_If_Found()
    {
        // Arrange
        var expectedManga = new Manga
        {
            Id = 1,
            Synopsis = "",
            Type = "Manga",
            CoverUrl = "url",
            MyAnimeListId = 1,
            Chapters = Enumerable.Empty<Chapter>(),
            UserMangas = Enumerable.Empty<UserManga>(),
            MangaGenres = Enumerable.Empty<MangaGenre>(),
            MangaSources = Enumerable.Empty<MangaSource>(),
            MangaAuthors = Enumerable.Empty<MangaAuthor>(),
            MangaTitles = Enumerable.Empty<MangaTitle>()
        };
        var sampleManga = new List<Manga>
        {
            expectedManga,
            new()
            {
                Id = 2,
                Synopsis = "",
                Type = "Manga",
                CoverUrl = "url",
                MyAnimeListId = 2,
                Chapters = Enumerable.Empty<Chapter>(),
                UserMangas = Enumerable.Empty<UserManga>(),
                MangaGenres = Enumerable.Empty<MangaGenre>(),
                MangaSources = Enumerable.Empty<MangaSource>(),
                MangaAuthors = Enumerable.Empty<MangaAuthor>(),
                MangaTitles = Enumerable.Empty<MangaTitle>()
            },
            new()
            {
                Id = 3,
                Synopsis = "",
                Type = "Manga",
                CoverUrl = "url",
                MyAnimeListId = 3,
                Chapters = Enumerable.Empty<Chapter>(),
                UserMangas = Enumerable.Empty<UserManga>(),
                MangaGenres = Enumerable.Empty<MangaGenre>(),
                MangaSources = Enumerable.Empty<MangaSource>(),
                MangaAuthors = Enumerable.Empty<MangaAuthor>(),
                MangaTitles = Enumerable.Empty<MangaTitle>()
            },
            new()
            {
                Id = 4,
                Synopsis = "",
                Type = "Manga",
                CoverUrl = "url",
                MyAnimeListId = 4,
                Chapters = Enumerable.Empty<Chapter>(),
                UserMangas = Enumerable.Empty<UserManga>(),
                MangaGenres = Enumerable.Empty<MangaGenre>(),
                MangaSources = Enumerable.Empty<MangaSource>(),
                MangaAuthors = Enumerable.Empty<MangaAuthor>(),
                MangaTitles = Enumerable.Empty<MangaTitle>()
            }
        };

        _context.Mangas.AddRange(sampleManga);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedManga, options =>
        {
            options.Excluding(c => c.Chapters);
            options.Excluding(c => c.UserMangas);
            options.Excluding(c => c.MangaGenres);
            options.Excluding(c => c.MangaTitles);
            options.Excluding(c => c.MangaAuthors);
            options.Excluding(c => c.MangaSources);
            return options;
        });
    }

    [Fact]
    public async Task GetByMalIdAsync_Should_Return_Manga_If_Found()
    {
        // Arrange
        var expectedManga = new Manga { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 1 };
        var sampleManga = new List<Manga>
        {
            expectedManga,
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 3 }
        };

        _context.Mangas.AddRange(sampleManga);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMalIdAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedManga);
    }

    [Fact]
    public async Task GetByMalIdAsync_Should_Return_Null_If_Not_Found()
    {
        // Arrange
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 3 }
        };

        _context.Mangas.AddRange(sampleManga);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMalIdAsync(4);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetWithFiltersAsync_Should_Return_List_Manga()
    {
        // Arrange
        var sampleMangaTitles = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1" },
            new() { Id = 2, MangaId = 2, Name = "Title2" },
            new() { Id = 3, MangaId = 3, Name = "Title3" },
            new() { Id = 4, MangaId = 4, Name = "Title4" }
        };
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 3 },
            new() { Id = 4, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 4 }
        };

        _context.Mangas.AddRange(sampleManga);
        _context.MangaTitles.AddRange(sampleMangaTitles);
        await _context.SaveChangesAsync();

        // Act
        var result = _repository.GetWithFiltersAsync("", null, null, null);

        // Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetWithFiltersAsync_Ordering_By_Alphabet_Should_Return_List_Manga()
    {
        // Arrange
        var sampleMangaTitles = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1" },
            new() { Id = 2, MangaId = 2, Name = "Title2" },
            new() { Id = 3, MangaId = 3, Name = "Title3" },
            new() { Id = 4, MangaId = 4, Name = "Title4" }
        };
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 3 },
            new() { Id = 4, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 4 }
        };

        _context.Mangas.AddRange(sampleManga);
        _context.MangaTitles.AddRange(sampleMangaTitles);
        await _context.SaveChangesAsync();

        // Act
        var result = _repository.GetWithFiltersAsync("alphabet", null, null, null);

        // Assert
        result.Should().HaveCount(4);
    }

    [Fact]
    public async Task GetWithFiltersAsync_Ordering_By_Latest_Should_Return_List_Manga()
    {
        // Arrange
        var sampleMangaSources = new List<MangaSource>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Url = "" },
            new() { Id = 2, MangaId = 2, SourceId = 1, Url = "" },
            new() { Id = 3, MangaId = 3, SourceId = 1, Url = "" },
            new() { Id = 4, MangaId = 4, SourceId = 1, Url = "" },
        };
        var sampleMangaGenres = new List<MangaGenre>
        {
            new() { Id = 1, MangaId = 1, GenreId = 1 },
            new() { Id = 2, MangaId = 1, GenreId = 2 },
            new() { Id = 3, MangaId = 2, GenreId = 1 },
            new() { Id = 4, MangaId = 2, GenreId = 4 },
            new() { Id = 5, MangaId = 3, GenreId = 1 },
            new() { Id = 6, MangaId = 3, GenreId = 3 },
            new() { Id = 7, MangaId = 4, GenreId = 4 },
            new() { Id = 8, MangaId = 5, GenreId = 3 },
        };
        var sampleMangaTitles = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1" },
            new() { Id = 2, MangaId = 2, Name = "Title2" },
            new() { Id = 3, MangaId = 3, Name = "Title3" },
            new() { Id = 4, MangaId = 4, Name = "Title4" }
        };
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 2 },
            new() { Id = 3, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 3 },
            new() { Id = 4, Synopsis = "", Type = "Manga", CoverUrl = "url", MyAnimeListId = 4 }
        };

        _context.Mangas.AddRange(sampleManga);
        _context.MangaTitles.AddRange(sampleMangaTitles);
        _context.MangaSources.AddRange(sampleMangaSources);
        _context.MangaGenres.AddRange(sampleMangaGenres);
        await _context.SaveChangesAsync();

        // Act
        var result = _repository.GetWithFiltersAsync("latest", new List<int> { 1 }, new List<int> { 1, 2 }, null);

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetByIdAndUserIdOrderedDescAsync_Should_Return_Manga_If_Found()
    {
        // Arrange
        var sampleSource = new Source { Id = 1, Name = "Source1", BaseUrl = "url" };
        var chapterList = new List<Chapter>()
        {
            new()
            {
                Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1",
                Source = sampleSource
            },
            new()
            {
                Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2",
                Source = sampleSource
            },
            new()
            {
                Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3",
                Source = sampleSource
            },
        };
        var sampleManga = new Manga
        {
            Id = 1,
            Synopsis = "",
            Type = "Manga",
            CoverUrl = "url",
            MyAnimeListId = 1,
            Chapters = chapterList,
            UserMangas = new List<UserManga>()
            {
                new() { Id = 1, MangaId = 1, UserId = "1" }
            },
            MangaGenres = Enumerable.Empty<MangaGenre>(),
            MangaSources = Enumerable.Empty<MangaSource>(),
            MangaAuthors = Enumerable.Empty<MangaAuthor>(),
            MangaTitles = Enumerable.Empty<MangaTitle>()
        };
        var expectedChapterList = new List<Chapter>()
        {
            new()
            {
                Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3",
                Source = sampleSource
            },
            new()
            {
                Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2",
                Source = sampleSource
            },
            new()
            {
                Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1",
                Source = sampleSource
            }
        };

        _context.Mangas.Add(sampleManga);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAndUserIdOrderedDescAsync(1, "1");

        // Assert
        Assert.NotNull(result);
        result.Chapters.Should().HaveCount(3);
        result.Chapters.Should().BeEquivalentTo(expectedChapterList, options =>
        {
            options.Excluding(c => c.Source);
            options.Excluding(c => c.Manga);
            return options;
        });
    }

    [Fact]
    public async Task GetByIdOrderedDescAsync_Should_Return_Manga_If_Found()
    {
        // Arrange
        var chapterList = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2" },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3" }
        };
        var sampleSource = new Source
        {
            Id = 1,
            Name = "Source1",
            BaseUrl = "",
        };
        var sampleManga = new Manga
        {
            Id = 1,
            Synopsis = "",
            Type = "Manga",
            CoverUrl = "url",
            MyAnimeListId = 1
        };
        var expectedChapterList = new List<Chapter>
        {
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2" },
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1" }
        };

        _context.Sources.AddRange(sampleSource);
        _context.Mangas.Add(sampleManga);
        _context.Chapters.AddRange(chapterList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdOrderedDescAsync(1);

        // Assert
        Assert.NotNull(result);
        result.Chapters.Should().HaveCount(3);
        result.Chapters.Should().BeEquivalentTo(expectedChapterList, options =>
        {
            options.Excluding(c => c.Source);
            options.Excluding(c => c.Manga);
            return options;
        });
    }
}