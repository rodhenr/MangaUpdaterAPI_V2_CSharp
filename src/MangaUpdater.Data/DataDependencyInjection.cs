using Microsoft.Extensions.DependencyInjection;

namespace MangaUpdater.Data;

public static class DataDependencyInjection
{
    public static IServiceCollection AddDataService(this IServiceCollection services)
    {
        services.AutoRegister();

        return services;
    } 
}