using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.User.Commands;
using MangaUpdater.IntegrationTests.Setup;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using InvalidOperationException = MangaUpdater.Exceptions.InvalidOperationException;

namespace MangaUpdater.IntegrationTests.FeaturesTests.User;

public class UpdateUserPasswordTests : BaseFixture, IAsyncLifetime
{
    private readonly IntegrationTestWebAppFactory _factory;
    private AppUser _user = null!;
    private const string UserId = "UserId";
    private const string Email = "someemail@email.com";

    public UpdateUserPasswordTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Should_Update_Password_When_User_Is_Valid()
    {
        // Arrange
        const string newPassword = $"{UserPassword}1";
        var command = new UpdateUserPasswordCommand(newPassword, UserPassword);
        AddUserIntoHttpContext(UserId);
        
        // Act
        await Sender.Send(command);
        
        // Assert
        var scope = _factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var isPasswordUpdated = await userManager.CheckPasswordAsync(_user, newPassword);
        
        Assert.True(isPasswordUpdated);
    }
        
    public new async Task InitializeAsync() => await SeedDb();
    
    private async Task SeedDb()
    {
        // User
        _user = await CreateUser(UserId);
    }
}