using System.Security.Claims;
using MangaUpdater.Core.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MangaUpdater.Core.Services;

[RegisterScoped]
public class CurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UserNotFoundException("User not found");
    
    public bool IsLoggedIn => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}