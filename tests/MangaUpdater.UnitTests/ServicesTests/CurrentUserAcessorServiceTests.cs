using System.Security.Claims;
using MangaUpdater.Exceptions;
using MangaUpdater.Services;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace MangaUpdater.UnitTests.ServicesTests;

public class CurrentUserAcessorServiceTests
{
    private readonly IHttpContextAccessor _contextAccessor;
    public CurrentUserAcessorServiceTests()
    {
        _contextAccessor = Substitute.For<IHttpContextAccessor>();
    }
    
    [Fact]
    public void UserId_Returns_Correct_UserId_When_User_Is_LoggedIn()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "SomeUserName") };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext { User = user };
        _contextAccessor.HttpContext.Returns(httpContext);
        
        var currentUserAccessor = new CurrentUserAccessor(_contextAccessor);

        // Act
        var userId = currentUserAccessor.UserId;

        // Assert
        Assert.Equal("SomeUserName", userId);
    }    
    
    [Fact]
    public void IsLoggedIn_Returns_True_When_User_Is_LoggedIn()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "SomeUserName") };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext { User = user };
        _contextAccessor.HttpContext.Returns(httpContext);
        
        var currentUserAccessor = new CurrentUserAccessor(_contextAccessor);

        // Act
        var isLoggedIn = currentUserAccessor.IsLoggedIn;

        // Assert
        Assert.True(isLoggedIn);
    }
    
    [Fact]
    public void IsLoggedIn_Returns_False_When_User_Is_Not_LoggedIn()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        
        var httpContext = new DefaultHttpContext { User = user };
        _contextAccessor.HttpContext.Returns(httpContext);

        var currentUserAccessor = new CurrentUserAccessor(_contextAccessor);

        // Act
        var isLoggedIn = currentUserAccessor.IsLoggedIn;

        // Assert
        Assert.False(isLoggedIn);
    }

    [Fact]
    public void UserId_Throws_UserNotFoundException_When_User_Is_Not_Found()
    {
        // Arrange
        HttpContext? httpContext = null;
        _contextAccessor.HttpContext.Returns(httpContext);

        var currentUserAccessor = new CurrentUserAccessor(_contextAccessor);

        // Act & Assert
        var exception = Assert.Throws<UserNotFoundException>(() => currentUserAccessor.UserId);
        Assert.Equal("User not found", exception.Message);
    }
}