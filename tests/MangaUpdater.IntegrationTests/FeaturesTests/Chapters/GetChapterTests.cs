using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Chapters.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class GetChapterTests : BaseFixture, IAsyncLifetime
{
    private Chapter _existingChapter = null!;
    private Manga _existingManga = null!;
    private Source _existingSource = null!;
    
    public GetChapterTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Get_Chapter_When_Chapter_Exists()
    {
        // Arrange
        var query = new GetChapterQuery(_existingManga.MyAnimeListId, _existingChapter.Id);

        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.Equal(_existingChapter.Id, result.Id);
        Assert.Equal(_existingManga.MyAnimeListId, result.MangaId);
        Assert.Equal(_existingSource.Id, result.SourceId);
        Assert.Equal(_existingChapter.Date, result.Date);
        Assert.Equal(_existingChapter.Number, result.Number);
    }
    
    [Fact]
    public async Task Should_ThrowException_When_Chapter_Not_Found()
    {
        // Arrange
        var mangaId = _existingManga.MyAnimeListId + 1;
        var chapterId = _existingChapter.Id + 1;
        
        var expectedExceptionMessage = $"Chapter not found for MangaId {mangaId} and ChapterId {chapterId}";
        
        var query = new GetChapterQuery(mangaId, chapterId);

        // Act && Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(async() => await Sender.Send(query));
        
        Assert.Equal(exception.Message, expectedExceptionMessage);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _existingManga = Fixture.Create<Manga>();
        _existingSource = Fixture.Create<Source>();
        
        await Insert(_existingManga);
        await Insert(_existingSource);
        
        _existingChapter = Fixture.Create<Chapter>();
        _existingChapter.MangaId = _existingManga.MyAnimeListId;
        _existingChapter.SourceId = _existingSource.Id;
        
        await Insert(_existingChapter);
    }
}