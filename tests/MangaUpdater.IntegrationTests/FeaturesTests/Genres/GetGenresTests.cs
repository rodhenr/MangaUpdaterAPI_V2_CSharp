using MangaUpdater.Features.Genres.Queries;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Genres;

public class GetGenresTests : BaseFixture, IAsyncLifetime
{
    private List<int> _genreIdsList = [1, 2];
    
    public GetGenresTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Genres()
    {
        // Arrange
        var query = new GetGenresQuery();
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_genreIdsList.Count, result.Count);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
        
    private async Task SeedDb()
    {
        // Mangas
        var mangaOne = Fixture.Create<Manga>();
        mangaOne.MyAnimeListId = 1;
        var mangaTwo = Fixture.Create<Manga>();
        mangaTwo.MyAnimeListId = 2;
        
        await InsertRange([mangaOne, mangaTwo]);

        // MangaGenres
        var mangaOneGenres = _genreIdsList
            .Select(id => new MangaGenre
            {
                MangaId = mangaOne.MyAnimeListId,
                GenreId = id
            }).ToList();
        
        await InsertRange(mangaOneGenres);
        
        var mangaTwoGenres = _genreIdsList
            .Select(id => new MangaGenre
            {
                MangaId = mangaTwo.MyAnimeListId,
                GenreId = id
            }).ToList();

        await InsertRange(mangaTwoGenres);
    }
}