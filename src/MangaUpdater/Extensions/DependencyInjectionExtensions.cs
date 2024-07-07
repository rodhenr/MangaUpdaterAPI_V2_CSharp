using System.Reflection;
using System.Text;
using CommunityToolkit.Diagnostics;
using FluentValidation;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.SqlServer;
using MangaUpdater.Features.Auth;
using MangaUpdater.Infrastructure;
using MangaUpdater.Infrastructure.Entities;
using MangaUpdater.Middlewares;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MangaUpdater.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddProblemDetails();
        
        return services;
    }

    public static WebApplication AddHangfireBuilder(this WebApplication builder, IConfiguration configuration)
    {
        var hangfireLogin = configuration["HangfireLogin"];
        var hangfirePassword = configuration["HangfirePassword"];
        
        Guard.IsNotNullOrEmpty(hangfireLogin);
        Guard.IsNotNullOrEmpty(hangfirePassword);
        
        builder.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[]
            {
                new BasicAuthAuthorizationFilter(
                    new BasicAuthAuthorizationFilterOptions()
                    {
                        RequireSsl = false,
                        SslRedirect = false,
                        LoginCaseSensitive = true,
                        Users = new[]
                        {
                            new BasicAuthAuthorizationUser
                            {
                                Login = hangfireLogin,
                                PasswordClear = hangfirePassword
                            }
                        }
                    })
            }
        });

        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var scheduledJobs = monitoringApi.ScheduledJobs(0, int.MaxValue);

        foreach (var job in scheduledJobs)
        {
            BackgroundJob.Delete(job.Key);
        }

        BackgroundJob.Enqueue<IHangfireService>(task => task.AddHangfireJobs(null));

        return builder;
    }
    
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AutoRegister();

        var executingAssembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(executingAssembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(executingAssembly);
        
        services.AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseIgnoredAssemblyVersionTypeResolver()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions(){ SchemaName =  "dbo" })
            .UseMediatR());
        services.AddHangfireServer(options => options.WorkerCount = 2);

        return services;
    }
    
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContextIdentity>(options => 
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly(typeof(AppDbContextIdentity).Assembly.FullName)));

        services.AddDefaultIdentity<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContextIdentity>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
        });

        return services;
    }
    
    public static IServiceCollection AddJwtAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var issuer = configuration["Issuer"];
        var audience = configuration["Audience"];
        var apiKey = configuration["ApiKey"];
        
        Guard.IsNotNullOrEmpty(issuer);
        Guard.IsNotNullOrEmpty(audience);
        Guard.IsNotNullOrEmpty(apiKey);

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(apiKey));
        
        // Configure JwtOptions
        services.Configure<JwtOptions>(options =>
        {
            options.Issuer = issuer;
            options.Audience = audience;
            options.SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);
            options.AccessTokenExpiration = 3000;
            options.RefreshTokenExpiration = 3600;
        });

        // Configure JWT
        services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; })
            .AddJwtBearer(options =>
            {
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        // Add policy
        services.AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("Typ", "Bearer")
                .Build())
            .AddPolicy("RefreshToken", policy => policy.RequireClaim("typ", "Refresh"))
            .AddPolicy("Admin", policy => policy.RequireRole("Admin"));

        return services;
    }
    
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        services.AutoRegister();

        return services;
    } 
}