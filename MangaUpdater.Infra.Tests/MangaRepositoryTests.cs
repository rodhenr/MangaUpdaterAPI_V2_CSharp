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
                new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 }
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
        var chapterList = new List<Chapter>()
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2" },
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3" },
        };
        var sampleManga = new Manga
        {
            Id = 1,
            Synopsis = "",
            Type = "Manga",
            CoverUrl = "url",
            MyAnimeListId = 1,
            Chapters = chapterList,
            MangaGenres = Enumerable.Empty<MangaGenre>(),
            MangaSources = Enumerable.Empty<MangaSource>(),
        };
        var expectedChapterList = new List<Chapter>()
        {
            new() { Id = 3, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-03-01"), Number = "3" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-02-01"), Number = "2" },
            new() { Id = 1, MangaId = 1, SourceId = 1, Date = DateTime.Parse("2023-01-01"), Number = "1" }
        };

        _context.Mangas.Add(sampleManga);
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