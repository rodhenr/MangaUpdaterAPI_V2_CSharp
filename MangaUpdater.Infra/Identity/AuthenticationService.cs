using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using MangaUpdater.Infra.Data.Identity;

namespace MangaUpdater.Infra.Data;

public class AuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<bool> Authenticate(UserAuthenticate userAuthenticate)
    {
        
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterUser(UserRegister userRegister)
    {
        var identityUser = new IdentityUser
        {
            UserName = userRegister.UserName,
            Email = userRegister.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(identityUser, userRegister.Password);

        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(identityUser, false);

        return result.Succeeded;
    }
}
