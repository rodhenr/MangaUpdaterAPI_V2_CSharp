using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Enums;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Chapters.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class UpdateChaptersFromAsuraScansTests : BaseFixture, IAsyncLifetime
{
    private Manga _existingManga = null!;
    private Source _asuraScansSource = null!;
    
    public UpdateChaptersFromAsuraScansTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Update_Chapters_For_AsuraScans_Source()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.AsuraScans;
        var command = new UpdateChaptersFromAsuraScansCommand(_existingManga.MyAnimeListId, (int)source, _asuraScansSource.BaseUrl);

        // Act
        await Sender.Send(command);

        // Assert
        var chapters = await Db.Chapters
            .Where(x => x.MangaId == _existingManga.MyAnimeListId && x.SourceId == (int)source)
            .ToListAsync();
        
        Assert.NotEmpty(chapters);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_MangaSource_Not_Exists()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.AsuraScans;
        var invalidMangaId = _existingManga.MyAnimeListId + 1;
            
        var exceptionMessage =
            $"MangaSource not found for MangaId {invalidMangaId} and SourceId {(int)source}";
        
        var command = new UpdateChaptersFromAsuraScansCommand(invalidMangaId, (int)source, _asuraScansSource.BaseUrl);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _existingManga = Fixture.Create<Manga>();
        await Insert(_existingManga);
        
        var mangaDexSource = new Source
        {
            Name = "Source1",
            BaseUrl = ""
        };
        
        _asuraScansSource = new Source
        {
            Name = "AsuraScans",
            BaseUrl = "https://asuracomic.net/series/"
        };
        
        await Insert(mangaDexSource);
        await Insert(_asuraScansSource);

        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.SourceId = _asuraScansSource.Id;
        mangaSource.MangaId = _existingManga.MyAnimeListId;
        mangaSource.Url = "swordmasters-youngest-son-3599edf0";
        
        await Insert(mangaSource);
        
    }
}