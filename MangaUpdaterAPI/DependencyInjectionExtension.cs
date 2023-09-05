using MangaUpdater.API.Exceptions;

namespace MangaUpdater.API;

public static class DependencyInjectionExtension
{
    public static IServiceCollection ConfigureInjection(this IServiceCollection services)
    {
        services.AddTransient<ExceptionMiddleware>();
        
        services.AddTransient<IExceptionHandler, AuthExceptionHandler>();
        services.AddTransient<IExceptionHandler, NotFoundExceptionHandler>();
        services.AddTransient<IExceptionHandler, ValidationExceptionHandler>();
        
        return services;
    }
}