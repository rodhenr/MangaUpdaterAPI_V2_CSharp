using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.User.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.User;

public class GetUserProfileInfoTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "userId";
    private AppUser _user = null!;
    public GetUserProfileInfoTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_User_Not_Exists()
    {
        // Arrange
        const string message = "User not found";
        
        var query = new GetUserProfileInfoQuery();
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(async() => await Sender.Send(query));
        Assert.Equal(message, exception.Message);
    }
    
    [Fact]
    public async Task Should_Return_Valid_And_Admin_User()
    {
        // Arrange
        var query = new GetUserProfileInfoQuery();
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_user.Id, result.Id);
        Assert.Equal(_user.UserName, result.Name);
        Assert.Equal(_user.Avatar, result.Avatar);
        Assert.Equal(_user.Email, result.Email);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // User
        _user = await CreateUser(UserId, true);
    }
}