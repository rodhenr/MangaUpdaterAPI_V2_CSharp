using MangaUpdater.Entities;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Features.UserMangas.Commands;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class DeleteFollowedSourceTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    private const string UserId = "UserId";
        
    public DeleteFollowedSourceTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Delete_UserChapter()
    {
        // Arrange
        var command = new DeleteFollowedSourceCommand(_manga.MyAnimeListId, _source.Id);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userChapter = await Db.UserMangas
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SelectMany(x => x.UserChapters.Where(y => y.SourceId == _source.Id))
            .SingleOrDefaultAsync();
        
        Assert.Null(userChapter);
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
        var chapter = Fixture.Create<Chapter>();

        chapter.MangaId = _manga.MyAnimeListId;
        chapter.SourceId = _source.Id;
        chapter.Date = DateTime.UtcNow;

        await Insert(chapter);

        // UserManga
        var userManga = new UserManga
        {
            MangaId = _manga.MyAnimeListId,
            UserId = "UserId"
        };

        await Insert(userManga);
        
        // UserChapters
        var userChapter = new UserChapter
        {
            SourceId = _source.Id,
            UserMangaId = userManga.Id,
            ChapterId = chapter.Id
        };

        await Insert(userChapter);
    }
}