using MangaUpdater.Features.Genres.Queries;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Entities;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Genres;

public class GetGenresTests : BaseFixture, IAsyncLifetime
{
    private List<Genre> _genreList = [];
    
    public GetGenresTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Genres()
    {
        // Arrange
        var query = new GetGenresQuery();
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_genreList.Count, result.Count);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
        
    private async Task SeedDb()
    {
        // Genres
        var genreOne = Fixture.Create<Genre>();
        genreOne.Id = 1;
        var genreTwo = Fixture.Create<Genre>();
        genreTwo.Id = 2;
        var genreThree = Fixture.Create<Genre>();
        genreThree.Id = 3;

        _genreList = [genreOne, genreTwo, genreThree];
        await InsertRange(_genreList);

        // Mangas
        var mangaOne = Fixture.Create<Manga>();
        var mangaTwo = Fixture.Create<Manga>();
        await InsertRange([mangaOne, mangaTwo]);

        // MangaGenres
        var mangaOneGenres = _genreList
            .Select(x => new MangaGenre
            {
                MangaId = mangaOne.MyAnimeListId,
                GenreId = x.Id
            }).ToList();
        
        var mangaTwoGenreOne = new MangaGenre
        {
            MangaId = mangaTwo.MyAnimeListId,
            GenreId = genreOne.Id
        };
        
        var mangaTwoGenreTwo = new MangaGenre
        {
            MangaId = mangaTwo.MyAnimeListId,
            GenreId = genreTwo.Id
        };

        await InsertRange([..mangaOneGenres, mangaTwoGenreOne, mangaTwoGenreTwo]);
    }
}