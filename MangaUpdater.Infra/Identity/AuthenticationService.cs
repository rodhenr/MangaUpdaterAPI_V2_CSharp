using System.Globalization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Models.Login;
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
            return await GenerateToken(userAuthenticate.Email);

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

    private async Task<UserAuthenticateResponse> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new AuthorizationException("User not found");
        var tokenClaims = await GetClaims(user);
        var tokenExpirationDate = DateTime.Now.AddSeconds(_jwtOptions.Expiration);

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwt = new SecurityTokenDescriptor
        {
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            Subject = new ClaimsIdentity(tokenClaims),
            NotBefore = DateTime.Now,
            Expires = tokenExpirationDate,
            SigningCredentials = _jwtOptions.SigningCredentials
        };

        var token = tokenHandler.CreateToken(jwt);

        return new UserAuthenticateResponse(tokenExpirationDate, tokenHandler.WriteToken(token));
    }

    private async Task<IList<Claim>> GetClaims(IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var usCulture = new CultureInfo("en-US");

        claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString(usCulture)));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(usCulture)));

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        return claims;
    }
}