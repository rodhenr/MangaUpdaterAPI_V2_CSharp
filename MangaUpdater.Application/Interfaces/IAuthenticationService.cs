using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces;
public interface IAuthenticationService
{
    Task<bool> Register(UserRegister userRegister);
    Task<UserAuthenticateResponse> Authenticate(UserAuthenticate userAuthenticate);
}
