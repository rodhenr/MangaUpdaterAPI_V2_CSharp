﻿using AutoMapper;
using MangaUpdater.Core.Dtos;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Data.Entities;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
    public async Task GetWithFilter_Should_Return_MangaDataWithPagesDto()
    {
        // Arrange
        var sampleMangaList = new List<Manga>
        {
            new()
            {
                Id = 1,
                Synopsis = "",
                Type = "",
                CoverUrl = "",
                MyAnimeListId = 1,
                MangaTitles = new List<MangaTitle>
                {
                    new() { Id = 1, Name = "Manga1", MangaId = 1 }
                }
            },
            new()
            {
                Id = 2,
                Synopsis = "",
                Type = "",
                CoverUrl = "",
                MyAnimeListId = 2,
                MangaTitles = new List<MangaTitle>
                {
                    new() { Id = 2, Name = "Manga2", MangaId = 2 }
                }
            },
            new()
            {
                Id = 3,
                Synopsis = "",
                Type = "",
                CoverUrl = "",
                MyAnimeListId = 3,
                MangaTitles = new List<MangaTitle>
                {
                    new() { Id = 3, Name = "Manga3", MangaId = 3 }
                }
            },
            new()
            {
                Id = 4,
                Synopsis = "",
                Type = "",
                CoverUrl = "",
                MyAnimeListId = 4,
                MangaTitles = new List<MangaTitle>
                {
                    new() { Id = 4, Name = "Manga4", MangaId = 4 }
                }
            },
        }.AsQueryable();

        var expected =
            new MangaDataWithPagesDto(
                new List<MangaUserDto>
                {
                    new(1, "", "Manga1"),
                    new(2, "", "Manga2"),
                    new(3, "", "Manga3"),
                    new(4, "", "Manga4"),
                }, 1);

        _repository
            .Setup(repo => repo.GetWithFiltersQueryable(It.IsAny<string>(), It.IsAny<List<int>>(),
                It.IsAny<List<int>>(),
                It.IsAny<string>()))
            .Returns(sampleMangaList);

        // Act
        var result = await _service.GetWithFilter(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(),
            It.IsAny<List<int>>(), It.IsAny<List<int>>(), It.IsAny<string>());

        // Assert
        result.Should().BeEquivalentTo(expected);
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
                    UserChapter = new List<UserChapter> { new() { UserMangaId = 1, SourceId = 1, ChapterId = 1 } }
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
        var mangaDto = new MangaDto
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
                    Read = true,
                    IsUserAllowedToRead = true
                }
            }
        };
        var highlightedMangas = new List<Manga>
        {
            new Manga
            {
                Id = 2,
                CoverUrl = "cover2",
                Synopsis = "synopsis",
                Type = "Manga",
                MyAnimeListId = 1,
                Chapters = Enumerable.Empty<Chapter>(),
                MangaTitles = new List<MangaTitle>()
                {
                    new() { Id = 3, MangaId = 2, Name = "Manga2", },
                    new() { Id = 4, MangaId = 2, Name = "AltManga2" }
                },
                MangaAuthors = Enumerable.Empty<MangaAuthor>(),
                UserMangas = Enumerable.Empty<UserManga>(),
                MangaGenres = Enumerable.Empty<MangaGenre>(),
                MangaSources = Enumerable.Empty<MangaSource>()
            }
        };
        var mangaUserDto = new List<MangaUserDto>
        {
            new(2, "cover2", "Manga2")
        };
        var expected = new MangaDataWithHighlightedMangasDto(mangaDto, mangaUserDto);

        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()))
            .ReturnsAsync(sampleManga);

        _repository
            .Setup(repo => repo.GetHighlightedAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(highlightedMangas);

        // Act
        var result = await _service.GetByIdNotLogged(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MangaDataWithHighlightedMangasDto>(result);
        _repository.Verify(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()), Times.Once);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetByIdNotLogged_Should_Throw_Exception_When_Manga_Is_Null()
    {
        // Arrange
        _repository
            .Setup(repo => repo.GetByIdOrderedDescAsync(It.IsAny<int>()))
            .ReturnsAsync(() => null);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.GetByIdNotLogged(It.IsAny<int>(), It.IsAny<int>()));
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
                    UserChapter = new List<UserChapter> { new() { UserMangaId = 1, SourceId = 1, ChapterId = 1 } }
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
        var mangaDto = new MangaDto
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
                    Read = true,
                    IsUserAllowedToRead = true
                }
            }
        };
        var expected = new MangaDataWithHighlightedMangasDto(mangaDto, Enumerable.Empty<MangaUserDto>());

        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());

        // Assert
        _repository.Verify(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        result.Should().BeEquivalentTo(expected);
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
            _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()));
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
        var mangaDto = new MangaDto
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
        var expected = new MangaDataWithHighlightedMangasDto(mangaDto, Enumerable.Empty<MangaUserDto>());

        _repository
            .Setup(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(sampleManga);

        // Act
        var result = await _service.GetByIdAndUserId(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>());

        // Assert
        _repository.Verify(repo => repo.GetByIdAndUserIdOrderedDescAsync(It.IsAny<int>(), It.IsAny<string>()),
            Times.Once);
        result.Should().BeEquivalentTo(expected);
    }
}