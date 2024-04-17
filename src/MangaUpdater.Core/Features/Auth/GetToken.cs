using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Core.Models;
using MangaUpdater.Core.Services;
using MangaUpdater.Data.Entities;

namespace MangaUpdater.Core.Features.Auth;

public record GetTokenQuery([FromQuery] UserAuthenticateModel UserAuthenticateModel) : IRequest<GetTokenResponse>;
public record GetTokenResponse(UserAuthenticateResponseModel UserAuthenticateResponse);

public sealed class GetTokenHandler : IRequestHandler<GetTokenQuery, GetTokenResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public GetTokenHandler(UserManager<AppUser> userManager, IOptions<JwtOptions> jwtOptions, CurrentUserAcessor currentUserAcessor)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<GetTokenResponse> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        var authenticationResponse = new UserAuthenticateResponseModel();

        var user = await _userManager.FindByIdAsync(userId);

        if (user?.Email is null || !authenticationResponse.IsSuccess)
            throw new AuthenticationException("User not found");

        return await GenerateCredentials(user.Email);
    }
    
    private async Task<GetTokenResponse> GenerateCredentials(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user?.Email is null) throw new AuthenticationException("User not found");

        var accessTokenClaims = await GetClaims(user, addUserClaims: true);
        var refreshTokenClaims = await GetClaims(user, addUserClaims: false);

        var accessTokenExpirationData = DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration);
        var refreshTokenExpirationData = DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration);

        var accessToken = GenerateToken(accessTokenClaims, accessTokenExpirationData);
        var refreshToken = GenerateToken(refreshTokenClaims, refreshTokenExpirationData);

        var isUserAdmin = await IsUserAdmin(user);

        return new GetTokenResponse(new UserAuthenticateResponseModel(user.UserName, user.Avatar, accessToken, refreshToken, isUserAdmin));
    }

    private string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate)
    {
        var jwt = new SecurityTokenDescriptor()
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

    private async Task<IList<Claim>> GetClaims(AppUser user, bool addUserClaims)
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

    private async Task<bool> IsUserAdmin(AppUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        return userRoles.Any(ur => ur == "Admin");
    }
}