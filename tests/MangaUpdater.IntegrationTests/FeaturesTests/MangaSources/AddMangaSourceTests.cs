using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.MangaSources.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.MangaSources;

public class AddMangaSourceTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    
    public AddMangaSourceTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Throw_Exception_When_Manga_Not_Exists()
    {
        // Arrange
        var mangaId = _manga.MyAnimeListId + 1;
        var sourceInfo = new SourceInfo(_source.Id, "");
        var exceptionMessage = $"Manga not found for ID {mangaId}.";
        
        var command = new AddMangaSourceCommand(mangaId, sourceInfo);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Throw_Exception_When_Source_Not_Exists()
    {
        // Arrange
        var sourceId = _source.Id + 1;
        var sourceInfo = new SourceInfo(sourceId, "");
        var exceptionMessage = $"Source not found for ID {sourceId}.";
        
        var command = new AddMangaSourceCommand(_manga.MyAnimeListId, sourceInfo);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Add_MangaSource_When_MangaId_And_SourceId_Are_Valid()
    {
        // Arrange
        const string url = "url";
        var sourceInfo = new SourceInfo(_source.Id, url);
        var command = new AddMangaSourceCommand(_manga.MyAnimeListId, sourceInfo);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var mangaSource = await Db.MangaSources
            .Where(x => x.MangaId == _manga.MyAnimeListId && x.SourceId == _source.Id)
            .FirstOrDefaultAsync();

        Assert.NotNull(mangaSource);
        Assert.Equal(_manga.MyAnimeListId, mangaSource.MangaId);
        Assert.Equal(_source.Id, mangaSource.SourceId);
        Assert.Equal(url, mangaSource.Url);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Sources
        _manga = Fixture.Create<Manga>();
        _source = Fixture.Create<Source>();
        
        await Insert(_manga);
        await Insert(_source);
    }
}