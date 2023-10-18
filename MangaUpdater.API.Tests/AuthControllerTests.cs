using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MangaUpdater.API.Controllers;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Models.Login;
using MangaUpdater.Application.Models.Register;

namespace MangaUpdater.API.Tests;

public class AuthControllerTests
{
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "testUser") };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "test"));

        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _authController = new AuthController(_authenticationServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            }
        };
    }

    [Fact]
    public async Task UserRegister_Should_Return_Success_When_User_Is_Valid()
    {
        var userRegister = new UserRegister();

        _authenticationServiceMock
            .Setup(s => s.Register(userRegister))
            .ReturnsAsync(new UserRegisterResponse());

        var result = await _authController.UserRegister(userRegister);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UserRegisterResponse>(okResult.Value);

        Assert.True(response.Success);
        Assert.Empty(response.ErrorList);
    }

    [Fact]
    public async Task UserLogin_Should_Return_Success_When_User_Is_Valid()
    {
        var userAuthenticate = new UserAuthenticate();

        _authenticationServiceMock
            .Setup(s => s.Authenticate(userAuthenticate))
            .ReturnsAsync(new UserAuthenticateResponse("example", "", "AccessToken", "RefreshToken"));

        var result = await _authController.UserLogin(userAuthenticate);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<UserAuthenticateResponse>(okResult.Value);

        Assert.Empty(response.ErrorList);
    }

    [Fact]
    public async Task RefreshToken_Should_Return_Success_When_User_Is_Valid()
    {
        var userAuthenticateResponse = new UserAuthenticateResponse();

        _authenticationServiceMock
            .Setup(s => s.RefreshToken(It.IsAny<string>()))
            .ReturnsAsync(userAuthenticateResponse);

        var result = await _authController.RefreshToken();

        Assert.IsType<ActionResult<UserAuthenticateResponse>>(result);
    }

    [Fact]
    public async Task RefreshToken_Should_Return_Unauthorized_When_User_Is_Invalid()
    {
        var userAuthenticateResponse = new UserAuthenticateResponse();
        userAuthenticateResponse.AddError("Invalid user.");

        _authenticationServiceMock
            .Setup(s => s.RefreshToken(It.IsAny<string>()))
            .ReturnsAsync(userAuthenticateResponse);

        var result = await _authController.RefreshToken();

        Assert.IsType<UnauthorizedResult>(result.Result);
    }
}