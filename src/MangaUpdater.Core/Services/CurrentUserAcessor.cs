using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace MangaUpdater.Core.Services;

[RegisterScoped]
public class CurrentUserAcessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAcessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}