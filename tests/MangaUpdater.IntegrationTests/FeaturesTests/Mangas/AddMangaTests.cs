using MangaUpdater.Features.Mangas.Commands;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class AddMangaTests: BaseIntegrationTest
{
    public AddMangaTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Should_Get_Chapter()
    {
        // Arrange
        const int id = 1;
        var query = new AddMangaCommand(id);

        // Act
        await Sender.Send(query);

        // Assert
        var manga = await DbContext.Mangas.Where(m => m.MyAnimeListId == id).FirstOrDefaultAsync();
        Assert.NotNull(manga);
    }
}