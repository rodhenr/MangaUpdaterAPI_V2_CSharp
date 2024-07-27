using AutoFixture;
using FluentAssertions;
using MangaUpdater.Entities;
using MangaUpdater.Features.UserMangas.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class UpdateFollowedSourcesTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private List<Source> _sources = null!;
    private const string UserId = "UserId";
        
    public UpdateFollowedSourcesTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Add_UserChapters()
    {
        // Arrange
        var sourcesToAdd = _sources.Select(x => x.Id).ToList();
        
        var command = new UpdateFollowedSourcesCommand(_manga.MyAnimeListId, sourcesToAdd);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userManga = await Db.UserMangas
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SingleOrDefaultAsync();

        Assert.NotNull(userManga);
        
        var userChaptersCount = await Db.UserChapters
            .Where(x => x.UserMangaId == userManga.Id)
            .CountAsync();
        
        Assert.Equal(sourcesToAdd.Count, userChaptersCount);
    }
    
    [Fact]
    public async Task Should_Remove_All_UserChapters_For_User()
    {
        // Arrange
        var command = new UpdateFollowedSourcesCommand(_manga.MyAnimeListId, []);
        AddUserIntoHttpContext(UserId);

        var newUserManga = new UserManga
        {
            MangaId = _manga.MyAnimeListId,
            UserId = UserId
            
        };

        await Insert(newUserManga);

        var newUserChapter = new UserChapter
        {
            SourceId = _sources.First().Id,
            UserMangaId = newUserManga.Id
        };
        
        await Insert(newUserChapter);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var userManga = await Db.UserMangas
            .Where(um => um.MangaId == _manga.MyAnimeListId && um.UserId == UserId)
            .SingleOrDefaultAsync();

        Assert.NotNull(userManga);
        
        var userChaptersCount = await Db.UserChapters
            .Where(x => x.UserMangaId == userManga.Id)
            .CountAsync();
        
        Assert.Equal(0, userChaptersCount);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);
        
        // Sources
        _sources = Fixture.CreateMany<Source>(3).ToList();
        await InsertRange(_sources);
        
        // MangaSource
        var mangaSources = Fixture.CreateMany<MangaSource>(3).ToList();

        for (var i = 0; i < _sources.Count; i++)
        {
            mangaSources[i].MangaId = _manga.MyAnimeListId;
            mangaSources[i].SourceId = _sources[i].Id;
        }

        await InsertRange(mangaSources);
    }
}