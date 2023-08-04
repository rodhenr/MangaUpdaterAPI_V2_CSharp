using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MangaUpdater.Infra.Context;
using MangaUpdater.Domain.Interfaces;
using MangaUpdater.Infra.Data.Repositories;
using MangaUpdater.Application.Services;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Mappings;

namespace MangaUpdater.Infra.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MangaUpdaterContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"
            ), b => b.MigrationsAssembly(typeof(MangaUpdaterContext).Assembly.FullName)));

        services.AddScoped<IMangaRepository, MangaRepository>();
        services.AddScoped<IMangaService, MangaService>();
        services.AddScoped<ISourceRepository, SourceRepository>();
        services.AddScoped<ISourceService, SourceService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IChapterRepository, ChapterRepository>();
        services.AddScoped<IChapterService, ChapterService>();
        services.AddScoped<IMangaSourceRepository, MangaSourceRepository>();
        services.AddScoped<IMangaSourceService, MangaSourceService>();
        services.AddScoped<IUserMangaRepository, UserMangaRepository>();
        services.AddScoped<IUserMangaService, UserMangaService>();
        services.AddScoped<IUserSourceService, UserSourceService>();
        services.AddScoped<IUserMangaChapterService, UserMangaChapterService>();

        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}