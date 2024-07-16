using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Features.Sources.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Sources;

public class GetSourcesTests : BaseFixture, IAsyncLifetime
{
    private List<Source> _sourceList = null!; 
    
    public GetSourcesTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_List_Of_Sources()
    {
        // Arrange
        var query = new GetSourcesQuery();
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_sourceList.Count, result.Count);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _sourceList = Fixture.CreateMany<Source>(3).ToList();

        await InsertRange(_sourceList);
    }
}