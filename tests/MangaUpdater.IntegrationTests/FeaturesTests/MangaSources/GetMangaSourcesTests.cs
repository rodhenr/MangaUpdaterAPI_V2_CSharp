using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.MangaSources.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.MangaSources;

public class GetMangaSourcesTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private List<Source> _sources = [];
    private const int NumberOfSources = 3;
    private const string UserId = "someUserId";
    
    public GetMangaSourcesTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Source_Info_With_User_Following_Some_Sources()
    {
        // Arrange
        var query = new GetMangaSourcesQuery(_manga.MyAnimeListId);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var sourcesUserIsFollowing = result.Where(x => x.IsUserFollowing).ToList();
        
        Assert.Equal(NumberOfSources, result.Count);
        Assert.Equal(NumberOfSources - 1, sourcesUserIsFollowing.Count);
    }
    
    [Fact]
    public async Task Should_Return_Source_Info_Without_Following_Any_Source()
    {
        // Arrange
        var query = new GetMangaSourcesQuery(_manga.MyAnimeListId);
        AddUserIntoHttpContext("anotherUserId");
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var isFollowingAny = result.Any(x => x.IsUserFollowing);
        
        Assert.Equal(NumberOfSources, result.Count);
        Assert.False(isFollowingAny);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Sources
        _manga = Fixture.Create<Manga>();
        _sources = Fixture.CreateMany<Source>(NumberOfSources).ToList();
        
        await Insert(_manga);
        await InsertRange(_sources);

        // MangaSources
        var mangaSources = Fixture.CreateMany<MangaSource>(NumberOfSources).ToList();

        for (var i = 0; i < _sources.Count; i++)
        {
            mangaSources[i].MangaId = _manga.MyAnimeListId;
            mangaSources[i].SourceId = _sources[i].Id;
        }
        
        await InsertRange(mangaSources);
        
        // UserManga
        var userManga = Fixture.Create<UserManga>();
        userManga.MangaId = _manga.MyAnimeListId;
        userManga.UserId = UserId;

        await Insert(userManga);
        
        // Chapters
        var chapters = Fixture.CreateMany<Chapter>(NumberOfSources).ToList();

        for (var i = 0; i < _sources.Count; i++)
        {
            chapters[i].MangaId = _manga.MyAnimeListId;
            chapters[i].SourceId = _sources[i].Id;
        }

        await InsertRange(chapters);

        // UserChapter
        var userChapters = Fixture.CreateMany<UserChapter>(NumberOfSources - 1).ToList();

        for (var i = 0; i < NumberOfSources - 1; i++)
        {
            userChapters[i].UserMangaId = userManga.Id;
            userChapters[i].SourceId = _sources[i].Id;
            userChapters[i].ChapterId = chapters[i].Id;
        }

        await InsertRange(userChapters);
    }
}