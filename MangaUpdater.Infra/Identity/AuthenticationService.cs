using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using MangaUpdater.Infra.Data.Identity;

namespace MangaUpdater.Infra.Data;

public class AuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _useManager;
    private readonly JwtOptions _jwtOptions;

    public AuthenticationService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> useManager, IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _useManager = useManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<bool> Authenticate(UserAuthenticate userAuthenticate)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RegisterUser(UserRegister userRegister)
    {
        throw new NotImplementedException();
    }
}
