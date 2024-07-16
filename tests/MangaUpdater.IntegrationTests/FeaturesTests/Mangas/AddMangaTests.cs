using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Mangas.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Mangas;

public class AddMangaTests: BaseFixture
{
    public AddMangaTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Add_Manga_When_Manga_Not_Exists()
    {
        // Arrange
        const int id = 1;
        var command = new AddMangaCommand(id);

        // Act
        await Sender.Send(command);

        // Assert
        var manga = await Db.Mangas.Where(m => m.MyAnimeListId == id).FirstOrDefaultAsync();
        Assert.NotNull(manga);
    }
    
    [Fact]
    public async Task Should_ThrowException_When_Manga_Exists()
    {
        // Arrange
        var manga = new Manga
        {
            MyAnimeListId = 2,
            Synopsis = "",
            Type = "",
            CoverUrl = "",
        };
        
        await Db.Mangas.AddAsync(manga);
        await Db.SaveChangesAsync();
        
        var expectedExceptionMessage = $"The ID {manga.MyAnimeListId} is already registered";
        
        var command = new AddMangaCommand(manga.MyAnimeListId);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(async() => await Sender.Send(command));
        
        Assert.Equal(exception.Message, expectedExceptionMessage);
    }
}