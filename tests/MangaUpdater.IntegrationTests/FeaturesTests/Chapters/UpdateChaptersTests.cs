using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Enums;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Chapters.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class UpdateChaptersTests : BaseFixture, IAsyncLifetime
{
    private Manga _existingManga = null!;
    
    public UpdateChaptersTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Update_Chapters_For_MangaDex_Source()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.MangaDex;
        
        var command = new UpdateChaptersCommand(_existingManga.MyAnimeListId, source);
        
        var mangaDexSource = new Source
        {
            Name = "MangaDex",
            BaseUrl = "https://api.mangadex.org/manga/"
        };
        await Insert(mangaDexSource);

        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.SourceId = mangaDexSource.Id;
        mangaSource.MangaId = _existingManga.MyAnimeListId;
        mangaSource.Url = "a1c7c817-4e59-43b7-9365-09675a149a6f";
        await Insert(mangaSource);

        // Act
        await Sender.Send(command);

        // Assert
        var chapters = await Db.Chapters
            .Where(x => x.MangaId == _existingManga.MyAnimeListId && x.SourceId == (int)source)
            .ToListAsync();
        
        Assert.NotEmpty(chapters);
    }
    
    [Fact]
    public async Task Should_Update_Chapters_For_AsuraScans_Source()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.AsuraScans;
        
        var mangaDexSource = new Source
        {
            Name = "MangaDex",
            BaseUrl = "https://api.mangadex.org/manga/"
        };
        var asuraScansSource = new Source
        {
            Name = "AsuraScans",
            BaseUrl = "https://asuratoon.com/manga/"
        };
        await Insert(mangaDexSource);
        await Insert(asuraScansSource);

        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.SourceId = asuraScansSource.Id;
        mangaSource.MangaId = _existingManga.MyAnimeListId;
        mangaSource.Url = "1908287720-swordmasters-youngest-son";
        await Insert(mangaSource);
        
        var command = new UpdateChaptersCommand(_existingManga.MyAnimeListId, source);

        // Act
        await Sender.Send(command);

        // Assert
        var chapters = await Db.Chapters
            .Where(x => x.MangaId == _existingManga.MyAnimeListId && x.SourceId == (int)source)
            .ToListAsync();
        
        Assert.NotEmpty(chapters);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Source_Not_Exists()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.MangaDex;
        var exceptionMessage = $"Source not found for ID {source}";
        
        var command = new UpdateChaptersCommand(_existingManga.MyAnimeListId, source);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _existingManga = Fixture.Create<Manga>();
        
        await Insert(_existingManga);
    }
}