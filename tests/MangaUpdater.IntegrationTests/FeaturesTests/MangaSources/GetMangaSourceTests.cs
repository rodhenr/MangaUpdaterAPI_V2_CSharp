using MangaUpdater.Entities;
using MangaUpdater.Features.MangaSources.Queries;
using MangaUpdater.IntegrationTests.Setup;
using AutoFixture;
using MangaUpdater.Exceptions;

namespace MangaUpdater.IntegrationTests.FeaturesTests.MangaSources;

public class GetMangaSourceTests : BaseFixture, IAsyncLifetime
{
    private Manga _manga = null!;
    private Source _source = null!;
    private MangaSource _mangaSource = null!;
    
    public GetMangaSourceTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_MangaSource_When_Exists()
    {
        // Arrange
        var query = new GetMangaSourceQuery(_manga.MyAnimeListId, _source.Id);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_mangaSource.Id, result.Id);
        Assert.Equal(_mangaSource.MangaId, result.MangaId);
        Assert.Equal(_mangaSource.SourceId, result.SourceId);
        Assert.Equal(_mangaSource.Url, result.Url);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_MangaSource_Is_Null()
    {
        // Arrange
        var invalidMangaId = _manga.MyAnimeListId + 1;
        var exceptionMessage = $"MangaSource not found for MangaId {invalidMangaId} and SourceId {_source.Id}";
        
        var query = new GetMangaSourceQuery(invalidMangaId, _source.Id);
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await Sender.Send(query));
        
        // Assert
        Assert.Equal(exception.Message, exceptionMessage);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // Manga & Source
        _manga = Fixture.Create<Manga>();
        _source = Fixture.Create<Source>();
        
        await Insert(_manga);
        await Insert(_source);

        // MangaSource
        _mangaSource = Fixture.Create<MangaSource>();
        _mangaSource.MangaId = _manga.MyAnimeListId;
        _mangaSource.SourceId = _source.Id;
        
        await Insert(_mangaSource);
    }
}