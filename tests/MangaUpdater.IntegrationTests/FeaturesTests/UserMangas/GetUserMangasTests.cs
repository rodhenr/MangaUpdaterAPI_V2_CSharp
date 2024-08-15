using MangaUpdater.Entities;
using MangaUpdater.Features.UserMangas.Queries;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;

namespace MangaUpdater.IntegrationTests.FeaturesTests.UserMangas;

public class GetUserMangasTests : BaseFixture, IAsyncLifetime
{
    private const int NumberOfMangas = 3;
    private const string UserId = "someUserId";
    
    public GetUserMangasTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Ok()
    {
        // Arrange
        var query = new GetUserMangasQuery(UserId);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(NumberOfMangas, result.Count);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Source
        var mangas = Fixture.CreateMany<Manga>(NumberOfMangas).ToList();
        var source = Fixture.Create<Source>();
        
        await InsertRange(mangas);
        await Insert(source);
        
        // MangaTitle && MangaSource
        var mangaTitles = Fixture.CreateMany<MangaTitle>(NumberOfMangas).ToList();
        var mangaSources = Fixture.CreateMany<MangaSource>(NumberOfMangas).ToList();

        for (var i = 0; i < mangas.Count; i++)
        {
            mangaTitles[i].MangaId = mangas[i].MyAnimeListId;
            mangaSources[i].MangaId = mangas[i].MyAnimeListId;
            mangaSources[i].SourceId = source.Id;
        }

        await InsertRange(mangaTitles);
        await InsertRange(mangaSources);
        
        // Chapters
        var chapters = Fixture.CreateMany<Chapter>(NumberOfMangas * 4).ToList();

        for(var i = 0; i < chapters.Count; i++)
        {
            var mangaIndex = 0;

            if (i % 3 == 0)
            {
                mangaIndex = 2;
            } else if (i % 2 == 0)
            {
                mangaIndex = 1;
            }
            
            chapters[i].MangaId = mangas[mangaIndex].MyAnimeListId;
            chapters[i].SourceId = source.Id;
            chapters[i].Number = (i + 1).ToString();
            chapters[i].Date = DateTime.SpecifyKind(chapters[i].Date, DateTimeKind.Utc);
        }

        await InsertRange(chapters);

        // UserManga
        var userMangas = Fixture.CreateMany<UserManga>(NumberOfMangas).ToList();

        for (var i = 0; i < mangas.Count; i++)
        {
            userMangas[i].MangaId = mangas[i].MyAnimeListId;
            userMangas[i].UserId = UserId;
        }

        await InsertRange(userMangas);
    }
}