using MangaUpdater.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MangaUpdater.IntegrationTests;

public abstract class BaseIntegrationTest: IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly IMediator Sender;
    protected readonly AppDbContextIdentity DbContext;
    
    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        
        Sender = _scope.ServiceProvider.GetRequiredService<IMediator>();
        
        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContextIdentity>();
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();

    }
}