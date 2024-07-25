using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.Mangas.Queries;
using MangaUpdater.IntegrationTests.Setup;
using MangaUpdater.Mappers;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class GetMangaFollowersCountTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private List<UserManga> _userMangas = null!;
    
    public GetMangaFollowersCountTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Follower_Count()
    {
        // Arrange
        var query = new GetMangaFollowersCountQuery(_manga.MyAnimeListId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_manga.MyAnimeListId, result.MangaId);
        Assert.Equal(_userMangas.Count, result.Followers);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
        
    private async Task SeedDb()
    {
        // Manga && MangaAuthors
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);
        
        // User
        var user1 = await CreateUser("UserId1");
        var user2 = await CreateUser("UserId2");
        var user3 = await CreateUser("UserId3");

        var users = new List<AppUser>
        {
            user1, 
            user2, 
            user3
        };
        
        // UserManga
        _userMangas = Fixture.CreateMany<UserManga>(3).ToList();

        for (var i = 0; i < 3; i++)
        {
            _userMangas[i].MangaId = _manga.MyAnimeListId;
            _userMangas[i].UserId = users[i].Id;
        }
        
        await InsertRange(_userMangas);
    }
}