using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.Auth.Commands;
using MangaUpdater.IntegrationTests.Setup;

namespace MangaUpdater.IntegrationTests.FeaturesTests.Auth;

public class RegisterUserTests : BaseFixture, IAsyncLifetime
{
    private const string UserId = "someUserId";
    private AppUser _user = null!;
    private const string Username = "userName";
    private const string Email = "email@email.com";
    
    public RegisterUserTests(IntegrationTestWebAppFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_Authenticate_And_Get_Tokens()
    {
        // Arrange
        var command = new RegisterUserCommand(Username, Email, UserPassword, UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);

        // Assert
        var userExists = await CheckIfUserExists(Email);
        Assert.True(userExists);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Email_Is_Not_Available()
    {
        // Arrange
        const string exceptionMessage = "Email already taken.";
        var command = new RegisterUserCommand(_user.UserName!, _user.Email!, UserPassword, UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async() => await Sender.Send(command));
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        _user = await CreateUser(UserId);
    }
}