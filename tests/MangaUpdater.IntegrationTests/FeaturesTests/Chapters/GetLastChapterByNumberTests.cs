using System.Globalization;
using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.Chapters.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class GetLastChapterByNumberTests: BaseFixture, IAsyncLifetime
{
    private Manga _mangaWithChapter = null!;
    private Manga _mangaWithoutChapter = null!;
    private Source _existingSource = null!;
    private Chapter _lastChapter = null!;
    
    public GetLastChapterByNumberTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Number_When_Any_Chapter_Is_Found()
    {
        // Arrange
        var query = new GetLastChapterByNumberQuery(_mangaWithChapter.MyAnimeListId, _existingSource.Id);

        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.Equal(_lastChapter.Number, result.Number.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Should_Return_0_When_No_Chapter_Is_Found()
    {
        // Arrange
        var query = new GetLastChapterByNumberQuery(_mangaWithoutChapter.MyAnimeListId, _existingSource.Id);

        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.Equal(0, result.Number);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        var mangas = Fixture.CreateMany<Manga>(2).ToList();
        _existingSource = Fixture.Create<Source>();

        await InsertRange(mangas);
        await Insert(_existingSource);
        
        _mangaWithChapter = mangas.First();
        _mangaWithoutChapter = mangas.Last();
        
        var chapters = Fixture.CreateMany<Chapter>(2).ToList();
        var chapterOne = chapters.First();
        var chapterTwo = chapters.Last();
        
        chapterOne.MangaId = _mangaWithChapter.MyAnimeListId;
        chapterOne.SourceId = _existingSource.Id;
        chapterOne.Number = "1";
        chapterOne.Date = DateTime.UtcNow;
        chapterTwo.MangaId = _mangaWithChapter.MyAnimeListId;
        chapterTwo.SourceId = _existingSource.Id;
        chapterTwo.Number = "2";
        chapterTwo.Date = DateTime.UtcNow;
        
        await InsertRange(chapters);

        _lastChapter = chapters.Last();
    }
}