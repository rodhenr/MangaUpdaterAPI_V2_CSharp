using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MangaUpdater.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MangaUpdater.Features.Auth;

public record GenerateTokenQuery(AppUser User) : IRequest<GenerateTokenResponse>;

public record GenerateTokenResponse(string AccessToken, string RefreshToken);

public sealed class GenerateTokenHandler : IRequestHandler<GenerateTokenQuery, GenerateTokenResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    
    public GenerateTokenHandler(UserManager<AppUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<GenerateTokenResponse> Handle(GenerateTokenQuery request, CancellationToken cancellationToken)
    {
        var accessTokenClaims = await GetClaims(request.User, false);
        var refreshTokenClaims = await GetClaims(request.User, true);

        var accessToken = GenerateToken(accessTokenClaims, DateTime.Now.AddSeconds(_jwtOptions.AccessTokenExpiration));
        var refreshToken = GenerateToken(refreshTokenClaims, DateTime.Now.AddSeconds(_jwtOptions.RefreshTokenExpiration));

        return new GenerateTokenResponse(accessToken, refreshToken);
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

    private async Task<IList<Claim>> GetClaims(AppUser user, bool isRefreshToken)
    {
        var usCulture = new CultureInfo("en-US");
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString(usCulture)),
            new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(usCulture))
        };

        if (isRefreshToken)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Typ, "Refresh"));
            return claims;
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Typ, "Bearer"));
        claims.AddRange(userClaims);
        claims.AddRange(roles.Select(role => new Claim("role", role)));

        return claims;
    }
}