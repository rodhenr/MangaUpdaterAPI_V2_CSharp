using MangaUpdater.Infra.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MangaUpdater.Application.Interfaces;

namespace MangaUpdater.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<MangaUpdaterContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("RijsatDatabase"),
            b => b.MigrationsAssembly(typeof(MangaUpdaterContext).Assembly.FullName)), ServiceLifetime.Transient);

        services.AddScoped<IApplicationDBContext>(provider => provider.GetService<MangaUpdaterContext>());
        return services;
    }
}