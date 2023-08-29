using MangaUpdater.Application.Models;

namespace MangaUpdater.Application.Interfaces;

public interface IAuthenticationService
{
    Task<UserRegisterResponse> Register(UserRegister userRegister);
    Task<UserAuthenticateResponse> Authenticate(UserAuthenticate userAuthenticate);
}