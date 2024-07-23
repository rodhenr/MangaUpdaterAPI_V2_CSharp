using System.Data.Common;
using Hangfire;
using Hangfire.Server;
using MangaUpdater.Database;
using MangaUpdater.Services.Hangfire;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Respawn;
using Testcontainers.MsSql;

namespace MangaUpdater.IntegrationTests.Setup;

[CollectionDefinition(nameof(SharedTestCollection))]
public class SharedTestCollection : ICollectionFixture<IntegrationTestWebAppFactory>;

public class IntegrationTestWebAppFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder().Build();
    private DbConnection _connection = null!;
    private Respawner _respawner = null!;
    public AppDbContextIdentity DbContext { get; private set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //builder.UseEnvironment("Testing");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHostedService));
            services.RemoveAll(typeof(JobStorage));
            services.RemoveAll(typeof(IBackgroundProcess));
            services.RemoveAll(typeof(HangfireService));
            services.RemoveAll(typeof(BackgroundJobServer));
            services.RemoveAll(typeof(BackgroundJob));
            services.RemoveAll(typeof(BackgroundJobClient));
            services.RemoveAll(typeof(IRecurringJobManager));
            
            services.RemoveAll(typeof(DbContextOptions<AppDbContextIdentity>));
            
            services.AddDbContext<AppDbContextIdentity>(options =>
            {
                options.UseSqlServer(_container.GetConnectionString());
                options.EnableSensitiveDataLogging();
            });
        });
    }
    
    public async Task InitializeAsync() 
    {
        await _container.StartAsync();
        
        DbContext = Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContextIdentity>();
        await DbContext.Database.EnsureCreatedAsync();
            
        _connection = DbContext.Database.GetDbConnection();
        
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"],
            WithReseed = true
        });
    }

    public new async Task DisposeAsync()
    {
        await _connection.CloseAsync();
        await _container.DisposeAsync();
    }
    
    public async Task ResetDatabaseAsync() => await _respawner.ResetAsync(_connection);
}