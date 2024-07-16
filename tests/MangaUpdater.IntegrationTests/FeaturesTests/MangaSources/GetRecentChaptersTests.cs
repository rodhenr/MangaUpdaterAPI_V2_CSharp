using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.UserMangas.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.MangaSources;

public class GetRecentChaptersTests : BaseFixture, IAsyncLifetime
{
    private Manga _existingManga = null!;
    private Source _existingSource = null!;
    private List<Chapter> _chapterList = null!;
    private AppUser _existingUser = null!;

    public GetRecentChaptersTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Three_Last_Chapters_When_User_Is_Null()
    {
        // Arrange
        var query = new GetRecentChaptersQuery(_existingManga.MyAnimeListId, [_existingSource.Id]);
        
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
        var query = new GetRecentChaptersQuery(_existingManga.MyAnimeListId, [_existingSource.Id]);
        AddUser();
        
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
        _existingManga = Fixture.Create<Manga>();
        _existingSource = Fixture.Create<Source>();
        
        await Insert(_existingManga);
        await Insert(_existingSource);
        
        // Chapters
        _chapterList = Fixture.CreateMany<Chapter>(5).ToList();

        for(var i = 0; i < _chapterList.Count; i++)
        {
            _chapterList[i].MangaId = _existingManga.MyAnimeListId;
            _chapterList[i].SourceId = _existingSource.Id;
            _chapterList[i].Number = (i + 1).ToString();
        }

        await InsertRange(_chapterList);
        
        // User
        _existingUser = await CreateUser();

        // UserManga
        var userManga = new UserManga
        {
            MangaId = _existingManga.MyAnimeListId,
            UserId = _existingUser.Id
        };

        await Insert(userManga);
        
        // UserChapters
        var userChapter = new UserChapter
        {
            SourceId = _existingSource.Id,
            UserMangaId = userManga.Id,
            ChapterId = _chapterList.OrderBy(x => float.Parse(x.Number)).Last().Id
        };

        await Insert(userChapter);
    }
}