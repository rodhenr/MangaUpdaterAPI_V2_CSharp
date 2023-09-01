using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Scraping;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Application.Services.External;
using MangaUpdater.Application.Services.Scraping;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.ExternalServices;
using MangaUpdater.Infra.Data.Identity;
using MangaUpdater.Infra.Data.Repositories;

namespace MangaUpdater.Infra.IoC;

public static class ServicesInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMangaRepository, MangaRepository>();
        services.AddScoped<IMangaSourceRepository, MangaSourceRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<IUserMangaRepository, UserMangaRepository>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<IMangaSourceService, MangaSourceService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IUserMangaService, UserMangaService>();
        services.AddScoped<IUserSourceService, UserSourceService>();
        services.AddScoped<IUserMangaChapterService, UserMangaChapterService>();
        services.AddScoped<IMyAnimeListApiService, MyAnimeListApiService>();
        services.AddScoped<IRegisterMangaService, RegisterMangaService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUpdateChaptersService, UpdateChaptersService>();
        services.AddScoped<IRegisterSourceService, RegisterSourceService>();
        
        services.AddScoped<ChromeDriver>(provider =>
        {
            var driverOptions = new ChromeOptions()
            {
                BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"
            };

            driverOptions.AddArguments(new List<string>() { "headless", "disable-gpu" });

            return new ChromeDriver(driverOptions);
        });

        services.AddAutoMapper(typeof(MappingProfile));
    }
}