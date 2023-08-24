using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using MangaUpdater.Infra.Data.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace MangaUpdater.Infra.Data;

public class AuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<bool> Register(UserRegister userRegister)
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

        return result.Succeeded;
    }

    public async Task<UserAuthenticateResponse> Authenticate(UserAuthenticate userAuthenticate)
    {
        var result = await _signInManager.PasswordSignInAsync(userAuthenticate.Email, userAuthenticate.Password, false, false);

        if (result.Succeeded)
            return await GenerateToken(userAuthenticate.Email);

        return new UserAuthenticateResponse(null, null, false);
    }

    private async Task<UserAuthenticateResponse> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email) ?? throw new Exception("");

        var tokenClaims = await GetClaims(user);

        var tokenExpirationDate = DateTime.Now.AddSeconds(_jwtOptions.Expiration);

        var jwt = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: tokenClaims,
            notBefore: DateTime.Now,
            expires: tokenExpirationDate,
            signingCredentials: _jwtOptions.SigningCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new UserAuthenticateResponse(tokenExpirationDate, token, true);
    }

    private async Task<IList<Claim>> GetClaims(IdentityUser user)
    {
        var claims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

        foreach (var role in roles)
            claims.Add(new Claim("role", role));

        return claims;
    }
}
