using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Sources.Queries;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Sources;

public class GetSourceTests : BaseFixture, IAsyncLifetime
{
    private Source _existingSource = null!; 
    
    public GetSourceTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Source_When_Source_When_Exists()
    {
        // Arrange
        var query = new GetSourceQuery(_existingSource.Id);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_existingSource.Id, result.Id);
        Assert.Equal(_existingSource.Name, result.Name);
        Assert.Equal(_existingSource.BaseUrl, result.Url);
    }
    
    [Fact]
    public async Task Should_Throw_Exception_When_Source_Not_Exists()
    {
        // Arrange
        var invalidSourceId = _existingSource.Id + 1;
        var expectedMessage = $"Source not found for SourceId {invalidSourceId}.";
        
        var query = new GetSourceQuery(invalidSourceId);
        
        // Act & Assert
        var result = await Assert.ThrowsAsync<EntityNotFoundException>(async () => await Sender.Send(query));
        
        // Assert
        Assert.Equal(expectedMessage, result.Message);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _existingSource = Fixture.Create<Source>();

        await Insert(_existingSource);
    }
}