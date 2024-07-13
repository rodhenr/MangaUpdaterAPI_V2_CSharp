using AutoFixture;
using MangaUpdater.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MangaUpdater.IntegrationTests.Setup;

[Collection(nameof(SharedTestCollection))]
public abstract class BaseFixture : IAsyncLifetime
{
    private Func<Task> _resetDatabase;
    protected readonly AppDbContextIdentity Db;
    protected readonly Fixture Fixture;
    public ISender Sender { get; private set; }

    public BaseFixture(IntegrationTestWebAppFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        Db = factory.DbContext;
        Fixture = new Fixture();
        Fixture.Customize(new NoCircularReferencesCustomization());
        Fixture.Customize(new IgnoreVirtualMembersCustomization());
        Fixture.Customize(new NoIdSpecimenCustomization());

        var scope = factory.Services.CreateScope();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        Sender = sender;
    }

    public async Task Insert<T>(T entity) where T : class
    {
        await Db.AddAsync(entity);
        await Db.SaveChangesAsync();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => _resetDatabase();
}