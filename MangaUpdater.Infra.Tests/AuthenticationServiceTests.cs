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
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
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

        _userManagerMock = new Mock<UserManager<AppUser>>(
            new Mock<IUserStore<AppUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<AppUser>>().Object,
            Array.Empty<IUserValidator<AppUser>>(),
            Array.Empty<IPasswordValidator<AppUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<AppUser>>>().Object);

        _signInManagerMock = new Mock<SignInManager<AppUser>>(
            _userManagerMock.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            Options.Create(new IdentityOptions()),
            Mock.Of<ILogger<SignInManager<AppUser>>>(),
            Mock.Of<IAuthenticationSchemeProvider>(),
            Mock.Of<IUserConfirmation<AppUser>>());

        _authenticationService =
            new AuthenticationService(_signInManagerMock.Object, _userManagerMock.Object, jwtOptionsMock.Object);
    }

    [Fact]
    public async Task Register_Successful_Registration_Returns_UserRegisterResponse()
    {
        // Arrange
        var userRegister = new UserRegister { Email = "test@example.com", Password = "password" };

        _userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _userManagerMock
            .Setup(userManager => userManager.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()));

        // Act
        var result = await _authenticationService.Register(userRegister);

        // Assert
        Assert.Empty(result.ErrorList);
        _userManagerMock.Verify(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Register_Should_Throw_ValidationException_If_User_Registration_Fails()
    {
        // Arrange
        var userRegister = new UserRegister { Email = "test@example.com", Password = "password" };
        var identityResult = IdentityResult.Failed(new IdentityError[] { new() { Description = "Invalid Email" } });

        _userManagerMock
            .Setup(userManager => userManager.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act and Assert
        await Assert.ThrowsAsync<ValidationException>(async () => await _authenticationService.Register(userRegister));
    }

    [Fact]
    public async Task Authenticate_Successful_Authentication_Returns_UserAuthenticateResponse()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var appUser = new AppUser
        {
            Email = userAuthenticate.Email,
            Avatar = ""
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
            .ReturnsAsync(appUser);

        _userManagerMock
            .Setup(userManager => userManager.GetClaimsAsync(appUser))
            .ReturnsAsync(claimList);

        _userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(appUser))
            .ReturnsAsync(rolesList);

        // Act
        var result = await _authenticationService.Authenticate(userAuthenticate);

        // Assert
        Assert.Empty(result.ErrorList);
        _signInManagerMock.Verify(m => m.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false));
        _userManagerMock.Verify(m => m.FindByEmailAsync(userAuthenticate.Email));
        _userManagerMock.Verify(m => m.GetClaimsAsync(appUser));
        _userManagerMock.Verify(m => m.GetRolesAsync(appUser));
    }

    [Fact]
    public async Task Authenticate_Should_Throw_ValidationException_If_User_Not_Found()
    {
        // Arrange
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var appUser = new AppUser
        {
            Email = userAuthenticate.Email,
            Avatar = ""
        };

        _signInManagerMock
            .Setup(userManager => userManager.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
            .ReturnsAsync(SignInResult.Success);

        _userManagerMock
            .Setup(userManager => userManager.FindByEmailAsync(userAuthenticate.Email))
            .ReturnsAsync(appUser);

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
        var appUser = new AppUser
        {
            Email = userAuthenticate.Email,
            Avatar = ""
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
            .ReturnsAsync(appUser);

        _userManagerMock
            .Setup(userManager => userManager.FindByEmailAsync(userAuthenticate.Email))
            .ReturnsAsync(appUser);

        _userManagerMock
            .Setup(userManager => userManager.GetClaimsAsync(appUser))
            .ReturnsAsync(claimList);

        _userManagerMock
            .Setup(userManager => userManager.GetRolesAsync(appUser))
            .ReturnsAsync(rolesList);

        // Act
        var result = await _authenticationService.RefreshToken(userId);

        // Assert
        Assert.Empty(result.ErrorList);
        _userManagerMock.Verify(m => m.FindByIdAsync(userId));
        _userManagerMock.Verify(m => m.FindByEmailAsync(userAuthenticate.Email));
        _userManagerMock.Verify(m => m.GetClaimsAsync(appUser));
        _userManagerMock.Verify(m => m.GetRolesAsync(appUser));
    }

    [Fact]
    public async Task RefreshToken_Should_Throw_ValidationException_If_User_Doesnt_Exist()
    {
        // Arrange
        const string userId = "1";
        var userAuthenticate = new UserAuthenticate { Email = "test@example.com", Password = "password" };
        var appUser = new AppUser
        {
            Email = userAuthenticate.Email,
            Avatar = ""
        };

        _userManagerMock
            .Setup(userManager => userManager.FindByIdAsync(userId))
            .ReturnsAsync(appUser);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AuthenticationException>(async () =>
            await _authenticationService.RefreshToken(userId));
        Assert.Contains("User not found", exception.Message);
    }
}