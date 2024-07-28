using FluentAssertions;
using MangaUpdater.DTOs;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Mangas.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class GetMangaInfoFromMyAnimeListTests: BaseFixture
{
    public GetMangaInfoFromMyAnimeListTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Return_Manga_Info_When_Id_Is_Valid()
    {
        // Arrange
        const int malId = 13;
        const string type = "Manga";
        var titles = new List<TitleEntry>
        {
            new() { Title = "One Piece", Type = "Default" }, 
            new() { Title = "ONE PIECE", Type = "Japanese" }, 
            new() { Title = "One Piece", Type = "English" }
        };
        var genres = new List<MalCollection>
        {
            new()
            {
                MalId = 1,
                Name = "Action",
                Type = "manga",
                Url = "https://myanimelist.net/manga/genre/1/Action"
            },
            new()
            {
                MalId = 2,
                Name = "Adventure",
                Type = "manga",
                Url = "https://myanimelist.net/manga/genre/2/Adventure"
            },
            new()
            {
                MalId = 10,
                Name = "Fantasy",
                Type = "manga",
                Url = "https://myanimelist.net/manga/genre/10/Fantasy"
            },
        };
        var authors = new List<MalCollection>
        {
            new()
            {
                MalId = 1881,
                Name = "Oda, Eiichiro",
                Type = "people",
                Url = "https://myanimelist.net/people/1881/Eiichiro_Oda",
            }
        };
        
        var query = new GetMangaInfoFromMyAnimeListQuery(malId);
        
        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.NotEmpty(result.Synopsis);
        Assert.NotEmpty(result.CoverUrl);
        Assert.Equal(type, result.Type);
        result.Titles.Should().BeEquivalentTo(titles);
        result.Genres.Should().BeEquivalentTo(genres);
        result.Authors.Should().BeEquivalentTo(authors);
    }

    // [Fact]
    // public async Task Should_Thrown_Exception_When_Id_Is_Invalid()
    // {
    //     // Arrange
    //     const int malId = 0;
    //     var exceptionMessage = $"Invalid ID {malId} from MyAnimeList";
    //     
    //     var query = new GetMangaInfoFromMyAnimeListQuery(malId);
    //
    //     // Act & Assert
    //     var exception = await Assert.ThrowsAsync<BadRequestException>(async() => await Sender.Send(query));
    //     Assert.Equal(exceptionMessage, exception.Message);
    // }
}