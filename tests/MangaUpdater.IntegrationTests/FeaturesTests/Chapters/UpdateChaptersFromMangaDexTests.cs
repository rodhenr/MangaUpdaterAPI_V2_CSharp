using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Enums;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Chapters.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class UpdateChaptersFromMangaDexTests : BaseFixture, IAsyncLifetime
{
    private Manga _existingManga = null!;
    private Source _mangaDexSource = null!;
    
    public UpdateChaptersFromMangaDexTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Update_Chapters_For_AsuraScans_Source()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.MangaDex;
        
        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.SourceId = _mangaDexSource.Id;
        mangaSource.MangaId = _existingManga.MyAnimeListId;
        mangaSource.Url = "a1c7c817-4e59-43b7-9365-09675a149a6f";
        
        await Insert(mangaSource);
        
        var command = new UpdateChaptersFromMangaDexCommand(_existingManga.MyAnimeListId, (int)source, _mangaDexSource.BaseUrl);

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
        const MangaSourcesEnum source = MangaSourcesEnum.MangaDex;
            
        var exceptionMessage =
            $"MangaSource not found for MangaId {_existingManga.MyAnimeListId} and SourceId {(int)source}";
        
        var command = new UpdateChaptersFromMangaDexCommand(_existingManga.MyAnimeListId, (int)source, _mangaDexSource.BaseUrl);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Request_Is_Not_Successful()
    {
        // Arrange
        const MangaSourcesEnum source = MangaSourcesEnum.MangaDex;
        
        var mangaSource = Fixture.Create<MangaSource>();
        mangaSource.SourceId = _mangaDexSource.Id;
        mangaSource.MangaId = _existingManga.MyAnimeListId;
        mangaSource.Url = "someinvalidurl";
        
        await Insert(mangaSource);
        
        var exceptionMessage =
            $"Failed to retrieve data for MangaId `{_existingManga.MyAnimeListId}` from MangaDex";
        
        var command = new UpdateChaptersFromMangaDexCommand(_existingManga.MyAnimeListId, (int)source, _mangaDexSource.BaseUrl);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _existingManga = Fixture.Create<Manga>();
        await Insert(_existingManga);
        
        _mangaDexSource = new Source
        {
            Name = "MangaDex",
            BaseUrl = "https://api.mangadex.org/manga/"
        };
        
        await Insert(_mangaDexSource);
    }
}