using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.User.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using InvalidOperationException = MangaUpdater.Exceptions.InvalidOperationException;

namespace MangaUpdater.IntegrationTests.FeaturesTests.User;

public class UpdateUserEmailTests : BaseFixture, IAsyncLifetime
{
    private readonly IntegrationTestWebAppFactory _factory;
    private AppUser _user = null!;
    private const string UserId = "UserId";
    private const string Email = "someemail@email.com";

    public UpdateUserEmailTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Should_Update_Email_When_Password_Is_Valid()
    {
        // Arrange
        var command = new UpdateUserEmailCommand(Email, UserPassword, UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var userEmail = await userManager.GetEmailAsync(_user);
        
        Assert.Equal(Email, userEmail);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_User_Not_Found()
    {
        // Arrange
        const string exceptionMessage = "User not found";
        var command = new UpdateUserEmailCommand(Email, UserPassword, UserPassword);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<UserNotFoundException>(async() => await Sender.Send(command));
        
        Assert.Equal(exceptionMessage, exception.Message);
    }
    
    [Fact]
    public async Task Should_Thrown_Exception_When_Password_Is_Invalid()
    {
        // Arrange
        const string invalidPassword = $"{UserPassword}111";
        const string exceptionMessage = "Invalid password";
        
        var command = new UpdateUserEmailCommand(Email, invalidPassword, invalidPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act && Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async() => await Sender.Send(command));
        
        Assert.Equal(exceptionMessage, exception.Message);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // User
        _user = await CreateUser(UserId);
    }
}