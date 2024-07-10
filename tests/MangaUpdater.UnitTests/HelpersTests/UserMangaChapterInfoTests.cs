using MangaUpdater.DTOs;
using MangaUpdater.Entities;
using MangaUpdater.Helpers;

namespace MangaUpdater.UnitTests.HelpersTests;

public class UserMangaChapterInfoTests
{
    [Fact]
    public void GetUserChaptersState_AllChaptersRead_ReturnsCorrectStates()
    {
        // Arrange
        var source = new Source { Id = 1, Name = "Source1" };
        
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "2" }
        };

        var userMangaInfo = new List<UserMangaDto>{ new (1, 1, 1, "2") };

        // Act
        var result = UserMangaChapterInfo.GetUserChaptersState(chapters, userMangaInfo);

        // Assert
        var resultList = result.ToList();
        
        Assert.True(resultList[0].IsRead);
        Assert.True(resultList[1].IsRead);
    }

    [Fact]
    public void GetUserChaptersState_SomeChaptersNotRead_ReturnsCorrectStates()
    {
        // Arrange
        var source = new Source { Id = 1, Name = "Source1" };
        
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "2" }
        };

        var userMangaInfo = new List<UserMangaDto>{ new (1, 1, 1, "1") };

        // Act
        var result = UserMangaChapterInfo.GetUserChaptersState(chapters, userMangaInfo);

        // Assert
        var resultList = result.ToList();
        
        Assert.True(resultList[0].IsRead);
        Assert.False(resultList[1].IsRead);
    }

    [Fact]
    public void GetUserChaptersState_NoUserMangaInfo_ReturnsCorrectStates()
    {
        // Arrange
        var source = new Source { Id = 1, Name = "Source1" };
        
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 1, Source = source, Date = DateTime.Now, Number = "2" }
        };

        var userMangaInfo = new List<UserMangaDto>();

        // Act
        var result = UserMangaChapterInfo.GetUserChaptersState(chapters, userMangaInfo);

        // Assert
        var resultList = result.ToList();
        
        Assert.False(resultList[0].IsRead);
        Assert.False(resultList[1].IsRead);
    }

    [Fact]
    public void GetUserChaptersState_MultipleSources_ReturnsCorrectStates()
    {
        // Arrange
        var chapters = new List<Chapter>
        {
            new() { Id = 1, MangaId = 1, SourceId = 1, Source = new Source { Id = 1, Name = "Source1" }, Date = DateTime.Now, Number = "1" },
            new() { Id = 2, MangaId = 1, SourceId = 2, Source = new Source { Id = 2, Name = "Source2" }, Date = DateTime.Now, Number = "1" }
        };

        var userMangaInfo = new List<UserMangaDto>{ new (1, 1, 1, "1") };

        // Act
        var result = UserMangaChapterInfo.GetUserChaptersState(chapters, userMangaInfo);

        // Assert
        var resultList = result.ToList();
        
        Assert.True(resultList[0].IsRead);
        Assert.False(resultList[1].IsRead);
    }
}