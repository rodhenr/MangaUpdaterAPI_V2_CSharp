using MangaUpdater.Entities;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Features.UserMangas.Commands;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class FollowMangaTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private const string UserId = "UserId";
        
    public FollowMangaTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Create_UserManga()
    {
        // Arrange
        var command = new FollowMangaCommand(_manga.MyAnimeListId);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userChapter = await Db.UserMangas
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SingleOrDefaultAsync();
        
        Assert.NotNull(userChapter);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);
    }
}