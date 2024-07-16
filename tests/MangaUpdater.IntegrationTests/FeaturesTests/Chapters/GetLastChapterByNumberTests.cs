using System.Globalization;
using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.Chapters.Queries;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;

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
        _mangaWithChapter = Fixture.Create<Manga>();
        _mangaWithoutChapter = Fixture.Create<Manga>();
        _existingSource = Fixture.Create<Source>();
        
        await Insert(_mangaWithChapter);
        await Insert(_mangaWithoutChapter);
        await Insert(_existingSource);
        
        var chapter = Fixture.Create<Chapter>();
        chapter.MangaId = _mangaWithChapter.MyAnimeListId;
        chapter.SourceId = _existingSource.Id;
        chapter.Number = "1";
        
        _lastChapter = Fixture.Create<Chapter>();
        _lastChapter.MangaId = _mangaWithChapter.MyAnimeListId;
        _lastChapter.SourceId = _existingSource.Id;
        _lastChapter.Number = "2";
        
        await Insert(chapter);
        await Insert(_lastChapter);
    }
}