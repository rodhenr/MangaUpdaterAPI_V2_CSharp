using System.Security.Claims;
using AutoFixture;
using MangaUpdater.Database;
using MangaUpdater.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MangaUpdater.IntegrationTests.Setup;

[Collection(nameof(SharedTestCollection))]
public abstract class BaseFixture : IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly Func<Task> _resetDatabase;
    protected readonly AppDbContextIdentity Db;
    protected readonly Fixture Fixture;
    protected ISender Sender { get; private set; }

    protected BaseFixture(IntegrationTestWebAppFactory factory)
    {
        _resetDatabase = factory.ResetDatabaseAsync;
        Db = factory.DbContext;
        Fixture = new Fixture();
        Fixture.Customize(new NoCircularReferencesCustomization());
        Fixture.Customize(new IgnoreVirtualMembersCustomization());
        Fixture.Customize(new NoIdSpecimenCustomization());

        _scope = factory.Services.CreateScope();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
    }

    protected async Task Insert<T>(T entity) where T : class
    {
        await Db.AddAsync(entity);
        await Db.SaveChangesAsync();
    }

    protected async Task InsertRange<T>(IEnumerable<T> entityList) where T : class
    {
        await Db.AddRangeAsync(entityList);
        await Db.SaveChangesAsync();
    }

    protected async Task<AppUser> CreateUser(string userId, bool isAdmin = false)
    {
        const string validPassword = "1234567!Aa";
        var user = Fixture.Create<AppUser>();
        user.Id = userId;
        user.UserName = userId.Replace("Id", "");
        
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user, validPassword);

        if (!isAdmin) return user;
        
        var userRole = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await userRole.CreateAsync(new IdentityRole("Admin")); //Check
        await userManager.AddToRoleAsync(user, "Admin");

        return user;
    }

    protected void AddUserIntoHttpContext(string userId)
    {
        var context = new DefaultHttpContext();
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId) 
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        context.User = new ClaimsPrincipal(identity);

        var httpContextAccessor = _scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = context;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Db.ChangeTracker.Clear();
        await _resetDatabase();
    } 
}