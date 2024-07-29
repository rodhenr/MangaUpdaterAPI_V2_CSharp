using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Auth;

public class RefreshTokenTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private AppUser _user = null!;
    
    public RefreshTokenTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Generate_Access_And_Refresh_Token()
    {
        // Arrange
        var query = new RefreshTokenQuery();
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
    }
    
    [Fact]
    public async Task Should_ThrowException_When_User_Not_Found()
    {
        // Arrange
        const string expectedExceptionMessage = "User not found.";
        var query = new RefreshTokenQuery();
        AddUserIntoHttpContext("SomeRandomUserId");
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(async() => await Sender.Send(query));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _user = await CreateUser(UserId);
    }
}