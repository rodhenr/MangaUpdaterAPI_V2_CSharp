using MangaUpdater.Application.Models;
using MangaUpdater.Application.Models.Login;
using MangaUpdater.Application.Models.RefreshToken;
using MangaUpdater.Application.Models.Register;

namespace MangaUpdater.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<UserRegisterResponse> Register(UserRegister userRegister);
    Task<UserAuthenticateResponse> Authenticate(UserAuthenticate userAuthenticate);
    Task<UserAuthenticateResponse> RefreshToken(string userId);
}