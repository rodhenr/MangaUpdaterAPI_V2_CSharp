using MangaUpdater.Entities;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.IntegrationTests.FeaturesTests;

public class BaseTest : BaseFixture, IAsyncLifetime
{
    public BaseTest(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Return_Ok()
    {
        // Arrange
        //var query = new();
        
        // Act
        //var result = await Sender.Send(query);
        
        // Assert
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
    }
}