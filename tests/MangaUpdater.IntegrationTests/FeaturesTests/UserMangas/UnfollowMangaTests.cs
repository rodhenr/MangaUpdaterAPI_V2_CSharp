using MangaUpdater.Entities;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Features.UserMangas.Commands;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class UnfollowMangaTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Chapter _chapter = null!;
    private Source _source = null!;
    private const string UserId = "UserId";
        
    public UnfollowMangaTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Unfollow_Manga()
    {
        // Arrange
        var command = new UnfollowMangaCommand(_manga.MyAnimeListId);
        AddUserIntoHttpContext(UserId);

        var newUserManga = new UserManga
        {
            MangaId = _manga.MyAnimeListId,
            UserId = "UserId"
        };
        await Insert(newUserManga);
        
        var newUserChapter = new UserChapter
        {
            SourceId = _source.Id,
            UserMangaId = newUserManga.Id,
            ChapterId = _chapter.Id
        };
        await Insert(newUserChapter);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userManga = await Db.UserMangas
            .Include(x => x.UserChapters)
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SingleOrDefaultAsync();
        
        var userChapters = await Db.UserChapters
            .Where(x => x.UserMangaId == newUserManga.Id)
            .ToListAsync();
        
        Assert.Null(userManga);
        Assert.Empty(userChapters);
    }
    
    [Fact]
    public async Task Should_Return_When_UserManga_Not_Exists()
    {
        // Arrange
        var command = new UnfollowMangaCommand(_manga.MyAnimeListId);
        AddUserIntoHttpContext(UserId);

        // Act
        await Sender.Send(command);
        
        // Assert
        var userManga = await Db.UserMangas
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SingleOrDefaultAsync();
        
        Assert.Null(userManga);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Source
        _manga = Fixture.Create<Manga>();
        _source = Fixture.Create<Source>();
        
        await Insert(_manga);
        await Insert(_source);
        
        // Chapters
        _chapter = Fixture.Create<Chapter>();

        _chapter.MangaId = _manga.MyAnimeListId;
        _chapter.SourceId = _source.Id;
        _chapter.Date = DateTime.UtcNow;

        await Insert(_chapter);
    }
}