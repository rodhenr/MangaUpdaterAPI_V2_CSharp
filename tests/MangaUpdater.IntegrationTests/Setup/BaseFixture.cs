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
    protected const string UserPassword = "1234567!Aa";
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
        var user = Fixture.Create<AppUser>();
        user.Id = userId;
        user.UserName = userId.Replace("Id", "");
        user.Email = "email@example.com";
        
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        await userManager.CreateAsync(user, UserPassword);
        
        await userManager.SetLockoutEnabledAsync(user, false);

        if (!isAdmin) return user;
        
        var userRole = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        await userRole.CreateAsync(new IdentityRole("Admin")); //Check
        await userManager.AddToRoleAsync(user, "Admin");

        return user;
    }

    protected async Task<bool> CheckIfUserExists(string email)
    {
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var user = await userManager.FindByEmailAsync(email);

        return user is not null;
    }

    protected void AddUserIntoHttpContext(string userId)
    {
        var context = CreateHttpContextAccessor();
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId) 
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        context.HttpContext!.User = new ClaimsPrincipal(identity);
    }

    protected IHttpContextAccessor CreateHttpContextAccessor()
    {
        var context = new DefaultHttpContext();
        var httpContextAccessor = _scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
        httpContextAccessor.HttpContext = context;

        return httpContextAccessor;
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        Db.ChangeTracker.Clear();
        await _resetDatabase();
    } 
}