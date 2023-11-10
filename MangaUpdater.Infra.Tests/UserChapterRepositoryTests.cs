using MangaUpdater.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Context;
using MangaUpdater.Infra.Data.Repositories;

namespace MangaUpdater.Infra.Tests;

public class UserChapterRepositoryTests
{
    private readonly IdentityMangaUpdaterContext _context;
    private readonly IUserChapterRepository _repository;

    public UserChapterRepositoryTests()
    {
        var dbOptions =
            new DbContextOptionsBuilder<IdentityMangaUpdaterContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        _context = new IdentityMangaUpdaterContext(dbOptions.Options);
        _repository = new UserChapterRepository(_context);
    }

    [Fact]
    public async Task GetByUserMangaIdAsync_Should_Return_List_UserChapter()
    {
        // Arrange
        var sampleUserChapters = new List<UserChapter>
        {
            new() { SourceId = 1, UserMangaId = 1, ChapterId = 1 },
            new() { SourceId = 2, UserMangaId = 1, ChapterId = 2 },
            new() { SourceId = 1, UserMangaId = 3, ChapterId = 3 }
        };

        _context.UserChapters.AddRange(sampleUserChapters);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserMangaIdAsync(1);

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByUserMangaIdAndSourceIdAsync_Should_Return_UserChapter_If_Exists()
    {
        // Arrange
        var expectedUserChapter = new UserChapter { SourceId = 1, UserMangaId = 1, ChapterId = 1 };
        var sampleUserChapter = new List<UserChapter>
        {
            expectedUserChapter,
            new() { SourceId = 2, UserMangaId = 1, ChapterId = 2 },
            new() { SourceId = 1, UserMangaId = 3, ChapterId = 3 }
        };

        _context.UserChapters.AddRange(sampleUserChapter);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserMangaIdAndSourceIdAsync(1, 1);

        // Assert
        Assert.NotNull(result);
        result.Should().BeEquivalentTo(expectedUserChapter);
    }

    [Fact]
    public async Task DeleteAsync_With_One_Parameter_Should_Delete_UserChapters()
    {
        // Arrange
        var sampleUserChapters = new List<UserChapter>
        {
            new() { SourceId = 1, UserMangaId = 1, ChapterId = 1 },
            new() { SourceId = 2, UserMangaId = 1, ChapterId = 2 },
            new() { SourceId = 1, UserMangaId = 3, ChapterId = 3 }
        };

        _context.UserChapters.AddRange(sampleUserChapters);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(1);

        // Assert
        var userChapters = _context.UserChapters.ToList();
        userChapters.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAsync_With_Two_Parameters_Should_Delete_UserChapters()
    {
        // Arrange
        var sampleUserChapters = new List<UserChapter>
        {
            new() { SourceId = 1, UserMangaId = 1, ChapterId = 1 },
            new() { SourceId = 2, UserMangaId = 1, ChapterId = 2 },
            new() { SourceId = 1, UserMangaId = 3, ChapterId = 3 }
        };

        _context.UserChapters.AddRange(sampleUserChapters);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(1, 1);

        // Assert
        var userChapters = _context.UserChapters.ToList();
        userChapters.Should().HaveCount(2);
    }

    [Fact]
    public async Task DDeleteRangeAsync_Should_Delete_UserChapters()
    {
        // Arrange
        var sampleUserChapters = new List<UserChapter>
        {
            new() { SourceId = 1, UserMangaId = 1, ChapterId = 1 },
            new() { SourceId = 2, UserMangaId = 1, ChapterId = 2 },
            new() { SourceId = 1, UserMangaId = 3, ChapterId = 3 }
        };

        _context.UserChapters.AddRange(sampleUserChapters);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteRangeAsync(1, new List<int> { 1, 2 });

        // Assert
        var userChapters = _context.UserChapters.ToList();
        userChapters.Should().HaveCount(1);
    }
}