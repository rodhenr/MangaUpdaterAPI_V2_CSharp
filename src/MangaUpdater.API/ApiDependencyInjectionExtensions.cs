using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using MangaUpdater.Core.Services;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MangaUpdater.API;

public static class ApiDependencyInjectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddProblemDetails();
        
        return services;
    }

    public static WebApplication AddHangfireBuilder(this WebApplication builder)
    {
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
                                Login = "Admin",
                                PasswordClear = "123"
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

        BackgroundJob.Enqueue<IHangfireService>(task => task.AddHangfireJobs());

        return builder;
    }
}