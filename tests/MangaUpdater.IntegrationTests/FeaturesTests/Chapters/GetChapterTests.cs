using MangaUpdater.Features.Chapters.Queries;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Chapters;

public class GetChapterTests : BaseIntegrationTest
{
    public GetChapterTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
        
    }

    [Fact]
    public async Task Should_Get_Chapter()
    {
        // Arrange
        var query = new GetChapterQuery(1, 1);

        // Act
        var result = await Sender.Send(query);

        // Assert
    }
}