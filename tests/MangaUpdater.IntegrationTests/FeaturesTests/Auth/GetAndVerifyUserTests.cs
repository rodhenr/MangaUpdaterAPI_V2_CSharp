using AutoFixture;
using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Auth;

public class GetAndVerifyUserTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private AppUser _user = null!;
    
    public GetAndVerifyUserTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Return_Ok()
    {
        // Arrange
        var query = new GetAndVerifyUserQuery(UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act
        var result = await Sender.Send(query);

        // Assert
        Assert.Equal(_user, result.User);
    }
    
    [Fact]
    public async Task Should_ThrowException_When_User_Not_Found()
    {
        // Arrange
        const string expectedExceptionMessage = "User not found.";
        var query = new GetAndVerifyUserQuery(UserPassword);
        AddUserIntoHttpContext("SomeRandomUserId");
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(async() => await Sender.Send(query));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_ThrowException_When_Password_Is_Wrong()
    {
        // Arrange
        const string expectedExceptionMessage = "Invalid credentials.";
        var query = new GetAndVerifyUserQuery("UserPassword");
        AddUserIntoHttpContext(UserId);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async() => await Sender.Send(query));
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _user = await CreateUser(UserId);
    }
}