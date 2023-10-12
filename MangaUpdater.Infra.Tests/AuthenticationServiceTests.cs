using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MangaUpdater.Application.Models.Login;
using MangaUpdater.Application.Models.Register;
using MangaUpdater.Domain.Exceptions;
using MangaUpdater.Infra.Data.Identity;
using Microsoft.IdentityModel.Tokens;
using AuthenticationService = MangaUpdater.Infra.Data.Identity.AuthenticationService;
using ValidationException = MangaUpdater.Domain.Exceptions.ValidationException;

namespace MangaUpdater.Infra.Tests;

public class AuthenticationServiceTests
{
    private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        var sampleJwt = new JwtOptions
        {
            Issuer = "abc",
            Audience = "def",
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes("123456789123456789123456789")),
                SecurityAlgorithms.HmacSha512),
            AccessTokenExpiration = 60,
            RefreshTokenExpiration = 60
        };
        var jwtOptionsMock = new Mock<IOptions<JwtOptions>>();
        jwtOptionsMock.Setup(j => j.Value).Returns(sampleJwt);

        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<IdentityUser>>().Object,
            Array.Empty<IUserValidator<IdentityUser>>(),
            Array.Empty<IPasswordValidator<IdentityUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<IdentityUser>>>().Object);

        _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<ILogger<SignInManager<IdentityUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<IdentityUser>>());

        _authenticationService =
            new AuthenticationService(_signInManagerMock.Object, _userManagerMock.Object, jwtOptionsMock.Object);
    }

    [Fact]
    public async Task Register_Successful_Registration_Returns_UserRegisterResponse()
    {
        // Arrange
        var userRegister = new UserRegister { Email = "test@example.com", Password = "password" };

        _userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock
            .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()));

        // Act
        var result = await _authenticationService.Register(userRegister);

        // Assert
        Assert.Empty(result.ErrorList);
        _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Register_Should_Throw_ValidationException_If_User_Registration_Fails()
    {
        // Arrange
        var userRegister = new UserRegister { Email = "test@example.com", Password = "password" };
        var identityResult = IdentityResult.Failed(new IdentityError[] { new() { Description = "Invalid Email" } });

        _userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(async () => await _authenticationService.Register(userRegister));
    }

    [Fact]
    public async Task Authenticate_Successful_Authentication_Returns_UserAuthenticateResponse()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var identityUser = new IdentityUser(userAuthenticate.Email)
        {
            Email = userAuthenticate.Email
        };
        var claimList = new List<Claim>()
        {
            new(ClaimTypes.Name, "test@example.com"),
            new(ClaimTypes.Email, "test@example.com")
        };
        var rolesList = new List<string>
        {
            "Admin"
        };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock
            .Setup(userManager => userManager.FindByEmailAsync(userAuthenticate.Email))
            .ReturnsAsync(identityUser);

        _userManagerMock
            .Setup(userManager => userManager.GetClaimsAsync(identityUser))
            .ReturnsAsync(claimList);

        _userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(identityUser))
            .ReturnsAsync(rolesList);

        // Act
        var result = await _authenticationService.Authenticate(userAuthenticate);

        // Assert
        Assert.Empty(result.ErrorList);
        _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false));
        _userManagerMock.Verify(m => m.FindByEmailAsync(userAuthenticate.Email));
        _userManagerMock.Verify(m => m.GetClaimsAsync(identityUser));
        _userManagerMock.Verify(m => m.GetRolesAsync(identityUser));
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_User_Not_Found()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var identityUser = new IdentityUser(userAuthenticate.Email);

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock
            .Setup(userManager => userManager.FindByEmailAsync(userAuthenticate.Email))
            .ReturnsAsync(identityUser);

        // Assert
        var exception = await Assert.ThrowsAsync<AuthenticationException>(async () =>
            await _authenticationService.Authenticate(userAuthenticate));
        Assert.Contains("User not found", exception.Message);
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_Account_Blocked()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.LockedOut);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async () =>
            await _authenticationService.Authenticate(userAuthenticate));
        Assert.Contains("Account blocked", exception.Message);
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_Not_Allowed()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.NotAllowed);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async () =>
            await _authenticationService.Authenticate(userAuthenticate));
        Assert.Contains("Not allowed", exception.Message);
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_Requires_Two_Factor()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.TwoFactorRequired);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async () =>
            await _authenticationService.Authenticate(userAuthenticate));
        Assert.Contains("Require two factor", exception.Message);
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_Invalid_User_Or_Password()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthorizationException>(async () =>
            await _authenticationService.Authenticate(userAuthenticate));
        Assert.Contains("Invalid user/password", exception.Message);
    }

    [Fact]
    public async Task RefreshToken_Returns_UserAuthenticateResponse()
    {
        // Arrange
        const string userId = "1";
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var identityUser = new IdentityUser(userAuthenticate.Email)
        {
            Email = userAuthenticate.Email
        };
        var claimList = new List<Claim>()
        {
            new(ClaimTypes.Name, userAuthenticate.Email),
            new(ClaimTypes.Email, userAuthenticate.Email)
        };
        var rolesList = new List<string>
        {
            "Admin"
        };

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(identityUser);

        _userManagerMock
            .Setup(userManager => userManager.FindByEmailAsync(userAuthenticate.Email))
            .ReturnsAsync(identityUser);

        _userManagerMock
            .Setup(userManager => userManager.GetClaimsAsync(identityUser))
            .ReturnsAsync(claimList);

        _userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(identityUser))
            .ReturnsAsync(rolesList);

        // Act
        var result = await _authenticationService.RefreshToken(userId);

        // Assert
        Assert.Empty(result.ErrorList);
        _userManagerMock.Verify(m => m.FindByIdAsync(userId));
        _userManagerMock.Verify(m => m.FindByEmailAsync(userAuthenticate.Email));
        _userManagerMock.Verify(m => m.GetClaimsAsync(identityUser));
        _userManagerMock.Verify(m => m.GetRolesAsync(identityUser));
    }

    [Fact]
    public async Task RefreshToken_Should_Throw_ValidationException_If_User_Doesnt_Exist()
    {
        // Arrange
        const string userId = "1";
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var identityUser = new IdentityUser(userAuthenticate.Email);

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(identityUser);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthenticationException>(async () =>
            await _authenticationService.RefreshToken(userId));
        Assert.Contains("User not found", exception.Message);
    }
}