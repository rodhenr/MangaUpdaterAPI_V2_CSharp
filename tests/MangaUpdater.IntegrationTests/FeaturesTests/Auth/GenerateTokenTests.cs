using MangaUpdater.Entities;
using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Auth;

public class GenerateTokenTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private AppUser _user = null!;
    
    public GenerateTokenTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Generate_Tokens()
    {
        // Arrange
        var query = new GenerateTokenQuery(_user);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _user = await CreateUser(UserId);
    }
}