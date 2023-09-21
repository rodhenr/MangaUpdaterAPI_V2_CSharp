using System.Globalization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Models.Login;
using MangaUpdater.Application.Models.RefreshToken;
using MangaUpdater.Application.Models.Register;
using MangaUpdater.Domain.Exceptions;

namespace MangaUpdater.Infra.Data.Identity;

public class AuthenticationService : IAuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UserRegisterResponse> Register(UserRegister userRegister)
    {
        var identityUser = new IdentityUser
        {
            UserName = userRegister.Email,
            Email = userRegister.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, userRegister.Password);

        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(identityUser, false);

        var userResponse = new UserRegisterResponse(result.Succeeded);

        if (result.Succeeded || !result.Errors.Any()) return userResponse;

        userResponse.AddErrors(result.Errors.Select(r => r.Description));
        throw new ValidationException(userResponse.ToString());
    }

    public async Task<UserAuthenticateResponse> Authenticate(UserAuthenticate userAuthenticate)
    {
        var result =
            await _signInManager.PasswordSignInAsync(userAuthenticate.Email, userAuthenticate.Password, false, false);

        if (result.Succeeded)
            return await GenerateCredentials(userAuthenticate.Email);

        var authResponse = new UserAuthenticateResponse();

        if (result.IsLockedOut)
            authResponse.AddError("Account blocked");
        else if (result.IsNotAllowed)
            authResponse.AddError("Not allowed");
        else if (result.RequiresTwoFactor)
            authResponse.AddError("Require two factor");
        else
            authResponse.AddError("Invalid user/password");

        if (authResponse.ErrorList.Count > 0) throw new AuthorizationException(authResponse.ToString());

        return authResponse;
    }

    public async Task<UserAuthenticateResponse> RefreshToken(string userId)
    {
        var authenticationResponse = new UserAuthenticateResponse();

        var user = await _userManager.FindByIdAsync(userId);

        if (user?.Email is null) throw new AuthenticationException("User not found");

        if (await _userManager.IsLockedOutAsync(user)) authenticationResponse.AddError("This account is blocked");

        if (authenticationResponse.IsSuccess) return await GenerateCredentials(user.Email);

        return authenticationResponse;
    }

    private async Task<UserAuthenticateResponse> GenerateCredentials(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user?.Email is null) throw new AuthenticationException("User not found");

        var accessTokenClaims = await GetClaims(user, addUserClaims: true);
        var refreshTokenClaims = await GetClaims(user, addUserClaims: false);

        var accessTokenExpirationData = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
        var refreshTokenExpirationData = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

        var accessToken = GenerateToken(accessTokenClaims, accessTokenExpirationData);
        var refreshToken = GenerateToken(refreshTokenClaims, refreshTokenExpirationData);

        return new UserAuthenticateResponse(accessToken, refreshToken);
    }

    private string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate)
    {
        var jwt = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.Now,
            Expires = expirationDate,
            SigningCredentials = _jwtOptions.SigningCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(jwt);

        return tokenHandler.WriteToken(token);
    }

    private async Task<IList<Claim>> GetClaims(IdentityUser user, bool addUserClaims)
    {
        var claims = new List<Claim>();
        var usCulture = new CultureInfo("en-US");

        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString(usCulture)));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(usCulture)));

        if (!addUserClaims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Typ, "Refresh"));
            return claims;
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(userClaims);
        claims.Add(new Claim(JwtRegisteredClaimNames.Typ, "Bearer"));

        claims.AddRange(roles.Select(role => new Claim("role", role)));

        return claims;
    }
}