using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MangaUpdater.Infra.Context;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Repositories;
using MangaUpdater.Application.Services;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Infra.Data.ExternalServices;
using MangaUpdater.Application.Services.Scraping;
using OpenQA.Selenium.Chrome;
using MangaUpdater.Application.Interfaces.Scraping;

namespace MangaUpdater.Infra.IoC;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MangaUpdaterContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"
            ), b => b.MigrationsAssembly(typeof(MangaUpdaterContext).Assembly.FullName)));

        services.AddScoped<IMangaRepository, MangaRepository>();
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IMangaSourceRepository, MangaSourceRepository>();
        services.AddScoped<IMangaSourceService, MangaSourceService>();
        services.AddScoped<IUserMangaRepository, UserMangaRepository>();
        services.AddScoped<IUserMangaService, UserMangaService>();
        services.AddScoped<IUserSourceService, UserSourceService>();
        services.AddScoped<IUserMangaChapterService, UserMangaChapterService>();
        services.AddScoped<IMyAnimeListAPIService, MyAnimeListAPIService>();
        services.AddScoped<IRegisterMangaService, RegisterMangaService>();
        services.AddScoped<IUpdateChaptersService, UpdateChaptersService>();
        services.AddScoped<IRegisterSourceService, RegisterSourceService>();
        services.AddScoped<ChromeDriver>(provider =>
        {
            var driverOptions = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            driverOptions.AddArguments(new List<string>() { "headless", "disable-gpu" });

            return new ChromeDriver(driverOptions);
        });

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}