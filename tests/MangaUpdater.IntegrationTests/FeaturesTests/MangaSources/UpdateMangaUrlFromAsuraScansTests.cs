using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.MangaSources.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.MangaSources;

public class UpdateMangaUrlFromAsuraScansTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    
    public UpdateMangaUrlFromAsuraScansTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Update_Manga_Url()
    {
        // Arrange
        _source = new Source
        {
            Name = "AsuraScans",
            BaseUrl = "https://asuracomic.net/series/"
        };
        await Insert(_source);

        var newMangaSource = Fixture.Create<MangaSource>();
        newMangaSource.SourceId = _source.Id;
        newMangaSource.MangaId = _manga.MyAnimeListId;
        await Insert(newMangaSource);
        
        const string url = "https://asuracomic.net/series/swordmasters-youngest-son-6f7238c3";
        var command = new UpdateMangaUrlFromAsuraScansCommand();
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var mangaSource = await Db.MangaSources
            .AsNoTracking()
            .Where(x => x.MangaId == _manga.MyAnimeListId && x.SourceId == _source.Id)
            .FirstOrDefaultAsync();

        Assert.NotNull(mangaSource);
        Assert.Equal(url, $"{_source.BaseUrl}{mangaSource.Url}");
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Source_Not_Found()
    {
        // Arrange
        const string exceptionMessage = "Source not found.";
        var command = new UpdateMangaUrlFromAsuraScansCommand();
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga
        _manga = Fixture.Create<Manga>();
        await Insert(_manga);
        
        // MangaTitle
        var mangaTitle = Fixture.Create<MangaTitle>();
        mangaTitle.MangaId = _manga.MyAnimeListId;
        mangaTitle.IsAsuraMainTitle = true;
        mangaTitle.Name = "Swordmaster’s Youngest Son";
        
        await Insert(mangaTitle);
        
        // Source 1
        var source1 = Fixture.Create<Source>();
        await Insert(source1);
    }
}