using MangaUpdater.Entities;
using MangaUpdater.Features.UserMangas.Queries;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class GetUserMangaSourcesTests : BaseFixture, IAsyncLifetime
{
    private const int NumberOfSources = 3;
    private const string UserId = "someUserId";
    private Manga _manga = null!;
    private UserManga _userManga = null!;
    private List<Source> _sources = null!;
    private List<Chapter> _chapters = null!;
    
    public GetUserMangaSourcesTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Manga_Sources_Without_Following_Any()
    {
        // Arrange
        AddUserIntoHttpContext(UserId);
        var query = new GetUserMangaSourcesQuery(_manga.MyAnimeListId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var isFollowingAll = result.All(x => x.IsFollowing);
        Assert.Equal(NumberOfSources, result.Count);
        Assert.False(isFollowingAll);
    }
    
    [Fact]
    public async Task Should_Return_Manga_Sources_Following_All()
    {
        // Arrange
        AddUserIntoHttpContext(UserId);
        var query = new GetUserMangaSourcesQuery(_manga.MyAnimeListId);
        
        var userChapters = Fixture.CreateMany<UserChapter>(NumberOfSources).ToList();
        
        for (var i = 0; i < _sources.Count; i++)
        {
            var lastChapter = _chapters.Last(x => x.SourceId == _sources[i].Id);
            
            userChapters[i].UserMangaId = _userManga.Id;
            userChapters[i].SourceId = _sources[i].Id;
            userChapters[i].ChapterId = lastChapter.Id;
        }

        await InsertRange(userChapters);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        var isFollowingAll = result.All(x => x.IsFollowing);
        Assert.Equal(NumberOfSources, result.Count);
        Assert.True(isFollowingAll);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Source
        _manga = Fixture.Create<Manga>();
        _sources = Fixture.CreateMany<Source>(NumberOfSources).ToList();
        
        await Insert(_manga);
        await InsertRange(_sources);
        
        // MangaTitle && MangaSource
        var mangaSources = Fixture.CreateMany<MangaSource>(NumberOfSources).ToList();

        for (var i = 0; i < _sources.Count; i++)
        {
            mangaSources[i].MangaId = _manga.MyAnimeListId;
            mangaSources[i].SourceId = _sources[i].Id;
        }

        await InsertRange(mangaSources);
        
        // Chapters
        _chapters = Fixture.CreateMany<Chapter>(NumberOfSources * 4).ToList();

        for(var i = 0; i < _chapters.Count; i++)
        {
            var sourceIndex = 0;

            if (i % 3 == 0)
            {
                sourceIndex = 2;
            } else if (i % 2 == 0)
            {
                sourceIndex = 1;
            }
            
            _chapters[i].MangaId = _manga.MyAnimeListId;
            _chapters[i].SourceId = _sources[sourceIndex].Id;
            _chapters[i].Number = (i + 1).ToString();
            _chapters[i].Date = DateTime.SpecifyKind(_chapters[i].Date, DateTimeKind.Utc);
        }

        await InsertRange(_chapters);

        // UserManga
        _userManga = Fixture.Create<UserManga>();

        _userManga.MangaId = _manga.MyAnimeListId;
        _userManga.UserId = UserId;

        await Insert(_userManga);
    }
}