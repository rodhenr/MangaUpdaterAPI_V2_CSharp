using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MangaUpdater.Infra.Data.Identity;

namespace MangaUpdater.Infra.IoC;

public static class AuthenticationInjection
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtAppSettingsOptions = configuration.GetSection(nameof(JwtOptions));
        var securityKey =
            new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration.GetSection("JwtOptions:SecurityKey").Value!));

        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = jwtAppSettingsOptions[nameof(JwtOptions.Issuer)]!;
            options.Audience = jwtAppSettingsOptions[nameof(JwtOptions.Audience)]!;
            options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            options.AccessTokenExpiration = int.Parse(jwtAppSettingsOptions[nameof(JwtOptions.AccessTokenExpiration)] ?? "0");
            options.RefreshTokenExpiration = int.Parse(jwtAppSettingsOptions[nameof(JwtOptions.RefreshTokenExpiration)] ?? "0");
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration.GetSection("JwtOptions:Issuer").Value,
            ValidateAudience = true,
            ValidAudience = configuration.GetSection("JwtOptions:Audience").Value,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });
    }
}