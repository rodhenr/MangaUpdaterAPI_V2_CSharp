using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Interfaces.Background;
using MangaUpdater.Application.Interfaces.External;
using MangaUpdater.Application.Interfaces.External.AsuraScans;
using MangaUpdater.Application.Interfaces.External.MangaDex;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Application.Services.Background;
using MangaUpdater.Application.Services.External;
using MangaUpdater.Application.Services.External.MyAnimeList;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.ExternalServices.AsuraScans;
using MangaUpdater.Infra.Data.ExternalServices.MangaDex;
using MangaUpdater.Infra.Data.ExternalServices.MyAnimeList;
using MangaUpdater.Infra.Data.Identity;
using MangaUpdater.Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace MangaUpdater.Infra.IoC;

public static class ServicesInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IMangaGenreRepository, MangaGenreRepository>();
        services.AddScoped<IMangaRepository, MangaRepository>();
        services.AddScoped<IMangaSourceRepository, MangaSourceRepository>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<IUserMangaRepository, UserMangaRepository>();
        services.AddScoped<IMangaAuthorRepository, MangaAuthorRepository>();
        services.AddScoped<IMangaTitleRepository, MangaTitleRepository>();
        services.AddScoped<IUserChapterRepository, UserChapterRepository>();

        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMangaGenreService, MangaGenreService>();
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<IMangaSourceService, MangaSourceService>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IUserMangaService, UserMangaService>();
        services.AddScoped<IUserSourceService, UserSourceService>();
        services.AddScoped<IUserMangaChapterService, UserMangaChapterService>();
        services.AddScoped<IMangaAuthorService, MangaAuthorService>();
        services.AddScoped<IMangaTitleService, MangaTitleService>();
        services.AddScoped<IUserChapterService, UserChapterService>();
        services.AddScoped<IUserAccountService, UserAccountService>();

        services.AddScoped<IMyAnimeListApiService, MyAnimeListApiService>();
        services.AddScoped<IRegisterMangaFromMyAnimeListService, RegisterMangaFromMyAnimeListService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<IMangaDexApi, MangaDexApiService>();
        services.AddScoped<IAsuraScansApi, AsuraScansApiService>();

        services.AddScoped<IExternalSourceService, ExternalSourceService>();

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IHangfireService, HangfireService>();

        services.AddHangfire(c => c
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer();
    }
}