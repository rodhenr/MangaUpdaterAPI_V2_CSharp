using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Data.Entities;
using MangaUpdater.Data;
using MangaUpdater.Data.Repositories;

namespace MangaUpdater.Infra.Tests;

public class UserMangaRepositoryTests
{
    private readonly AppDbContextIdentity _context;
    private readonly IUserMangaRepository _repository;

    public UserMangaRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<AppDbContextIdentity>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new AppDbContextIdentity(dbOptions.Options);
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
            new() { Id = 1, MangaId = 1, UserId = "1" },
            new() { Id = 2, MangaId = 2, UserId = "1" },
            new() { Id = 3, MangaId = 1, UserId = "2" }
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
            new() { Id = 1, MangaId = 1, UserId = "1" },
            new() { Id = 2, MangaId = 2, UserId = "1" },
            new() { Id = 3, MangaId = 1, UserId = "2" }
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
    public async Task GetByMangaIdAndUserIdAsync_Should_Return_UserManga()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { MangaId = 1, UserId = "user1" },
            new() { MangaId = 1, UserId = "user2" },
            new() { MangaId = 2, UserId = "user1" }
        };
        var expected = new UserManga { Id = 1, MangaId = 1, UserId = "user1" };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdAndUserIdAsync(1, "user1");

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetByMangaIdAndUserIdAsync_Should_Return_Null()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { MangaId = 1, UserId = "user1" },
            new() { MangaId = 1, UserId = "user2" },
            new() { MangaId = 2, UserId = "user1" }
        };

        _context.UserMangas.AddRange(sampleUserMangas);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdAndUserIdAsync(2, "user2");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_With_Two_Parameters_Should_Delete_UserMangas()
    {
        // Arrange
        var sampleUserMangas = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, UserId = "1" },
            new() { Id = 2, MangaId = 2, UserId = "1" },
            new() { Id = 3, MangaId = 1, UserId = "2" },
            new() { Id = 4, MangaId = 1, UserId = "1" }
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
            new() { Id = 1, MangaId = 1, UserId = "1" },
            new() { Id = 2, MangaId = 2, UserId = "1" },
            new() { Id = 3, MangaId = 1, UserId = "2" },
            new() { Id = 4, MangaId = 1, UserId = "1" }
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
    public async Task GetByMangaIdAndUserIdAsync_Should_Return_UserManga_If_Found()
    {
        // Arrange
        var userMangaList = new List<UserManga>
        {
            new() { Id = 1, MangaId = 1, UserId = "user1" },
            new() { Id = 2, MangaId = 2, UserId = "user1" },
            new() { Id = 3, MangaId = 2, UserId = "user2" },
            new() { Id = 4, MangaId = 3, UserId = "user1" },
        };
        var expected = new UserManga { Id = 1, MangaId = 1, UserId = "user1" };

        _context.UserMangas.AddRange(userMangaList);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMangaIdAndUserIdAsync(1, "user1");

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Not_Modify_Id_And_SaveChanges()
    {
        // Arrange
        var userMangaSample = new UserManga { MangaId = 1, UserId = "user1"};
        _context.UserMangas.Add(userMangaSample);
        await _context.SaveChangesAsync();

        userMangaSample.MangaId = 2;
        
        // Act
        await _repository.SaveChangesAsync(userMangaSample);
        
        // Assert
        _context.Entry(userMangaSample).Property(x => x.Id).IsModified.Should().BeFalse();
        userMangaSample.MangaId.Should().Be(2);
    }
}