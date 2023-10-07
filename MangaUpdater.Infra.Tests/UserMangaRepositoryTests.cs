using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Domain.Entities;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infra.Tests;

public class UserMangaRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IUserMangaRepository _repository;

    public UserMangaRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new UserMangaRepository(_context);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_Should_Return_List_UserMangas()
    {
        // Arrange
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "", CoverUrl = "", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "", CoverUrl = "", MyAnimeListId = 2 }
        };
        var sampleMangaTitle = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1" },
            new() { Id = 2, MangaId = 2, Name = "Title2" }
        };
        var sampleSource = new Source { Id = 1, Name = "Source1", BaseUrl = "url" };
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 }
        };

        _context.Mangas.AddRange(sampleManga);
        _context.MangaTitles.AddRange(sampleMangaTitle);
        _context.UserMangas.AddRange(sampleUserMangas);
        _context.Sources.Add(sampleSource);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByUserIdAsync("1");

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_Should_Return_Empty_List()
    {
        // Arrange
        var sampleManga = new List<Manga>
        {
            new() { Id = 1, Synopsis = "", Type = "", CoverUrl = "", MyAnimeListId = 1 },
            new() { Id = 2, Synopsis = "", Type = "", CoverUrl = "", MyAnimeListId = 2 }
        };
        var sampleMangaTitle = new List<MangaTitle>
        {
            new() { Id = 1, MangaId = 1, Name = "Title1" },
            new() { Id = 2, MangaId = 2, Name = "Title2" }
        };
        var sampleSource = new Source { Id = 1, Name = "Source1", BaseUrl = "url" };
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 }
        };

        _context.Mangas.AddRange(sampleManga);
        _context.MangaTitles.AddRange(sampleMangaTitle);
        _context.UserMangas.AddRange(sampleUserMangas);
        _context.Sources.Add(sampleSource);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByUserIdAsync("3");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllByMangaIdAndUserIdAsync_Should_Return_List_UserMangas()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 },
            new() { Id = 4, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 4 },
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByMangaIdAndUserIdAsync(1, "1");

        // Assert
        Assert.NotNull(result);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllByMangaIdAndUserIdAsync_Should_Return_Empty_List()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 }
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllByMangaIdAndUserIdAsync(2, "2");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByMangaIdUserIdAndSourceIdAsync_Should_Return_UserManga()
    {
        // Arrange
        var expectedUserManga = new UserManga { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 };
        var sampleUserMangas = new List<UserManga>
        {
            expectedUserManga,
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 },
            new() { Id = 4, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 4 },
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdUserIdAndSourceIdAsync(1, "1", 1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedUserManga);
    }

    [Fact]
    public async Task GetByMangaIdUserIdAndSourceIdAsync_Should_Return_Null()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "2", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 },
            new() { Id = 4, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 4 },
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdUserIdAndSourceIdAsync(2, "1", 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_With_Two_Parameters_Should_Delete_UserMangas()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 },
            new() { Id = 4, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 4 }
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(1, "1");

        // Assert
        var userMangas = _context.UserMangas.ToList();
        userMangas.Should().HaveCount(2);
    }
    
    [Fact]
    public async Task DeleteAsync_With_Three_Parameters_Should_Delete_UserMangas()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, UserId = "1", CurrentChapterId = 1 },
            new() { Id = 2, MangaId = 2, SourceId = 1, UserId = "1", CurrentChapterId = 2 },
            new() { Id = 3, MangaId = 1, SourceId = 1, UserId = "2", CurrentChapterId = 3 },
            new() { Id = 4, MangaId = 1, SourceId = 2, UserId = "1", CurrentChapterId = 4 }
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(1, 1, "1");

        // Assert
        var userMangas = _context.UserMangas.ToList();
        userMangas.Should().HaveCount(3);
    }
}