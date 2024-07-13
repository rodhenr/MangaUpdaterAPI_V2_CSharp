using MangaUpdater.Features.Chapters.Queries;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class GetLastChapterByNumberTests
{
    public GetLastChapterByNumberTests() { }

    [Fact]
    public async Task Should_Return_Ok()
    {
        // Arrange
        
        var query = new GetLastChapterByNumberQuery(1, 1);

        // Act

        // Assert
    }
}