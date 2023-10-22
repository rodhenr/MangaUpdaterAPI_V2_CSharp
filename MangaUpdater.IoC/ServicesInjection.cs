using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Authentication;
using MangaUpdater.Application.Interfaces.External.MangaLivre;
using MangaUpdater.Application.Interfaces.External.MyAnimeList;
using MangaUpdater.Application.Mappings;
using MangaUpdater.Application.Services;
using MangaUpdater.Application.Services.External.MangaLivre;
using MangaUpdater.Application.Services.External.MyAnimeList;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.ExternalServices.MangaLivre;
using MangaUpdater.Infra.Data.ExternalServices.MyAnimeList;
using MangaUpdater.Infra.Data.Identity;
using MangaUpdater.Infra.Data.Repositories;

namespace MangaUpdater.Infra.IoC;

public static class ServicesInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
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
        
        services.AddScoped<IMyAnimeListApiService, MyAnimeListApiService>();
        services.AddScoped<IRegisterMangaFromMyAnimeListService, RegisterMangaFromMyAnimeListService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddScoped<IMangaLivreApi, MangaLivreApiService>();
        services.AddScoped<IMangaLivreService, MangaLivreService>();

        services.AddAutoMapper(typeof(MappingProfile));
    }
}