using Microsoft.IdentityModel.Tokens;

namespace MangaUpdater.Infra.Data.Identity;

public class JwtOptions
{
    public const string Section = "JwtOptions";

    public JwtOptions()
    {
    }

    public JwtOptions(string issuer, string audience, SigningCredentials signingCredentials, int accessTokenExpiration,
        int refreshTokenExpiration) : this()
    {
        Issuer = issuer;
        Audience = audience;
        SigningCredentials = signingCredentials;
        AccessTokenExpiration = accessTokenExpiration;
        RefreshTokenExpiration = refreshTokenExpiration;
    }

    public string Issuer { get; set; }
    public string Audience { get; set; }
    public SigningCredentials SigningCredentials { get; set; }
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
}