using MangaUpdater.Entities;
using MangaUpdater.Helpers;

namespace MangaUpdater.UnitTests.HelpersTests;

public class ChapterFixture
{
    public Chapter Chapter1 { get; }
    public Chapter Chapter2 { get; }
    public Chapter Chapter3 { get; }
        
    public ChapterFixture()
    {
        Chapter1 = new Chapter { Id = 1, MangaId = 1, SourceId = 1, Number = "1", Date = DateTime.Today };
        Chapter2 = new Chapter { Id = 2, MangaId = 1, SourceId = 1, Number = "1", Date = DateTime.Today };
        Chapter3 = new Chapter { Id = 3, MangaId = 1, SourceId = 2, Number = "2", Date = DateTime.Today };
    }
}

public class ChapterEqualityComparerTests : IClassFixture<ChapterFixture>
{
    private readonly ChapterEqualityComparer _chapterEqualityComparer;
    private readonly ChapterFixture _fixture;
    public ChapterEqualityComparerTests(ChapterFixture fixture)
    {
        _fixture = fixture;
        _chapterEqualityComparer = new ChapterEqualityComparer();
    }
    
    [Fact]
    public void Equals_SameChapters_ReturnsTrue()
    {
        // Act
        var result = _chapterEqualityComparer.Equals(_fixture.Chapter1, _fixture.Chapter2);
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_DifferentChapters_ReturnsFalse()
    {
        // Act
        var result = _chapterEqualityComparer.Equals(_fixture.Chapter1, _fixture.Chapter3);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_OneChapterIsNull_ReturnsFalse()
    {
        // Act
        var result = _chapterEqualityComparer.Equals(_fixture.Chapter1, null);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Equals_BothChaptersAreNull_ReturnsFalse()
    {
        // Act
        var result = _chapterEqualityComparer.Equals(null, null);
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetHashCode_SameChapters_ReturnsSameHashCode()
    {
        // Act
        var resultChapter1 = _chapterEqualityComparer.GetHashCode(_fixture.Chapter1);
        var resultChapter2 = _chapterEqualityComparer.GetHashCode(_fixture.Chapter2);
        
        // Assert
        Assert.Equal(resultChapter1, resultChapter2);
    }

    [Fact]
    public void GetHashCode_DifferentChapters_ReturnsDifferentHashCodes()
    {
        // Act
        var resultChapter1 = _chapterEqualityComparer.GetHashCode(_fixture.Chapter1);
        var resultChapter3 = _chapterEqualityComparer.GetHashCode(_fixture.Chapter3);
        
        // Assert
        Assert.NotEqual(resultChapter1, resultChapter3);
    }
}