using System.Security.Authentication;
using MangaUpdater.Entities;
using MangaUpdater.Features.User.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.User;

public class GetUserInfoTests : BaseFixture, IAsyncLifetime
{
    private const string NonAdminUserId = "nonAdminUserId";
    private const string AdminUserId = "adminUserId";
    private AppUser _nonAdminUser = null!;
    private AppUser _adminUser = null!;
    public GetUserInfoTests(IntegrationTestWebAppFactory factory) : base(factory) { }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_User_Not_Exists()
    {
        // Arrange
        const string message = "User not found";
        
        var query = new GetUserInfoQuery("someRandomAndInvalidUser");
        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<AuthenticationException>(async() => await Sender.Send(query));
        Assert.Equal(message, exception.Message);
    }
    
    [Fact]
    public async Task Should_Return_Valid_And_Non_Admin_User()
    {
        // Arrange
        var query = new GetUserInfoQuery(NonAdminUserId);
        AddUserIntoHttpContext(NonAdminUserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_nonAdminUser.Id, result.Id);
        Assert.Equal(_nonAdminUser.UserName, result.Name);
        Assert.Equal(_nonAdminUser.Avatar, result.Avatar);
        Assert.Equal(_nonAdminUser.Email, result.Email);
        Assert.False(result.IsAdmin);
    }
    
    [Fact]
    public async Task Should_Return_Valid_And_Admin_User()
    {
        // Arrange
        var query = new GetUserInfoQuery(AdminUserId);
        AddUserIntoHttpContext(AdminUserId);
        
        // Act
        var result = await Sender.Send(query);
        
        // Assert
        Assert.Equal(_adminUser.Id, result.Id);
        Assert.Equal(_adminUser.UserName, result.Name);
        Assert.Equal(_adminUser.Avatar, result.Avatar);
        Assert.Equal(_adminUser.Email, result.Email);
        Assert.True(result.IsAdmin);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // User
        _nonAdminUser = await CreateUser(NonAdminUserId);
        _adminUser = await CreateUser(AdminUserId, true);
    }
}