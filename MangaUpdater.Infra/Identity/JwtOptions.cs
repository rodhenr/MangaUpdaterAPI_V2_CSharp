using Microsoft.IdentityModel.Tokens;

namespace MangaUpdater.Infra.Data.Identity;

public class JwtOptions
{
    public JwtOptions(string issuer, string audience, SigningCredentials signingCredentials, int expiration)
    {
        Issuer = issuer;
        Audience = audience;
        SigningCredentials = signingCredentials;
        Expiration = expiration;
    }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public SigningCredentials SigningCredentials { get; set; }

    public int Expiration { get; set; }
}