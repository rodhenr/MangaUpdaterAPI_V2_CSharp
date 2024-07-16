using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.UserMangas.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class GetRecentChaptersTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    private List<Chapter> _chapters = null!;
    private const string UserId = "someUserId";

    public GetRecentChaptersTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Three_Last_Chapters_When_User_Is_Null()
    {
        // Arrange
        var query = new GetRecentChaptersQuery(_manga.MyAnimeListId, [_source.Id]);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var allUnread = result.All(x => !x.IsRead);
        var allNotAllowedToRead = result.All(x => !x.IsUserAllowedToRead);
        
        Assert.Equal(3, result.Count);
        Assert.True(allUnread);
        Assert.True(allNotAllowedToRead);
    }
    
    [Fact]
    public async Task Should_Return_Three_Last_Chapters_When_User_Is_Not_Null()
    {
        // Arrange
        var query = new GetRecentChaptersQuery(_manga.MyAnimeListId, [_source.Id]);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var allRead = result.All(x => x.IsRead);
        var allAllowedToRead = result.All(x => x.IsUserAllowedToRead);
        
        Assert.Equal(3, result.Count);
        Assert.True(allRead);
        Assert.True(allAllowedToRead);
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
        _chapters = Fixture.CreateMany<Chapter>(5).ToList();

        for(var i = 0; i < _chapters.Count; i++)
        {
            _chapters[i].MangaId = _manga.MyAnimeListId;
            _chapters[i].SourceId = _source.Id;
            _chapters[i].Number = (i + 1).ToString();
        }

        await InsertRange(_chapters);

        // UserManga
        var userManga = new UserManga
        {
            MangaId = _manga.MyAnimeListId,
            UserId = UserId
        };

        await Insert(userManga);
        
        // UserChapters
        var userChapter = new UserChapter
        {
            SourceId = _source.Id,
            UserMangaId = userManga.Id,
            ChapterId = _chapters.OrderBy(x => float.Parse(x.Number)).Last().Id
        };

        await Insert(userChapter);
    }
}