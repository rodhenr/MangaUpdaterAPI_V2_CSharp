using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.UserMangas.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class UpdateUserChapterTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    private Chapter _lastChapter = null!;
    private UserManga _userManga = null!;
    private const string UserId = "UserId";
        
    public UpdateUserChapterTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Chapter_Is_Invalid()
    {
        // Arrange
        const string exceptionMessage = "Invalid chapter.";
        var command = new UpdateUserChapterCommand(_manga.MyAnimeListId, _source.Id, _lastChapter.Id + 1);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_UserChapter_Is_Invalid()
    {
        // Arrange
        var newChapter = Fixture.Create<Chapter>();
        newChapter.MangaId = _manga.MyAnimeListId;
        newChapter.SourceId = _source.Id;
        newChapter.Date = DateTime.UtcNow;
        
        await Insert(newChapter);
        
        const string exceptionMessage = "UserChapter not found.";
        var command = new UpdateUserChapterCommand(_manga.MyAnimeListId, _source.Id, _lastChapter.Id);
        AddUserIntoHttpContext("InvalidUserId");
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Update_UserChapters()
    {
        // Arrange
        var newChapter = Fixture.Create<Chapter>();
        newChapter.MangaId = _manga.MyAnimeListId;
        newChapter.SourceId = _source.Id;
        newChapter.Date = DateTime.UtcNow;
        
        await Insert(newChapter);
        
        var command = new UpdateUserChapterCommand(_manga.MyAnimeListId, _source.Id, newChapter.Id);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userChapter = await Db.UserChapters
            .Where(x => x.SourceId == _source.Id && x.UserManga.UserId == UserId &&
                        x.UserManga.MangaId == _manga.MyAnimeListId)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        Assert.NotNull(userChapter);
        Assert.Equal(newChapter.Id, userChapter.ChapterId);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);
        
        // Source
        _source = Fixture.Create<Source>();
        await Insert(_source);
        
        // UserManga
        _userManga = Fixture.Create<UserManga>();
        _userManga.MangaId = _manga.MyAnimeListId;
        _userManga.UserId = UserId;
        
        await Insert(_userManga);
        
        // Chapter
        _lastChapter = Fixture.Create<Chapter>();
        _lastChapter.MangaId = _manga.MyAnimeListId;
        _lastChapter.SourceId = _source.Id;
        _lastChapter.Date = DateTime.UtcNow;
        
        await Insert(_lastChapter);
        
        // UserChapter
        var userChapter = Fixture.Create<UserChapter>();
        userChapter.UserMangaId = _userManga.Id;
        userChapter.SourceId = _source.Id;
        userChapter.ChapterId = _lastChapter.Id;
            
        await Insert(userChapter);
    }
}