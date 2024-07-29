using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Auth;

public class AuthenticateUserTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private AppUser _user = null!;
    
    public AuthenticateUserTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Authenticate_And_Get_Tokens()
    {
        // Arrange
        var query = new AuthenticateUserQuery(_user.Email!, UserPassword);
        //AddUserIntoHttpContext(UserId);
        CreateHttpContextAccessor();
        
        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_User_Not_Found()
    {
        // Arrange
        const string exceptionMessage = "User not found.";
        var query = new AuthenticateUserQuery("someRandomEmail@email.com", UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(async() => await Sender.Send(query));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _user = await CreateUser(UserId);
    }
}