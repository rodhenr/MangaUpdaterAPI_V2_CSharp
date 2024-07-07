using System.Security.Authentication;
using MangaUpdater.Infrastructure.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Features.Auth.Login;

public record GetUserInfoQuery(string UserId) : IRequest<GetUserInfoResponse>;
public record GetUserInfoResponse(string Avatar, string Name, string Id, string Email, bool IsAdmin);

public sealed class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    private readonly UserManager<AppUser> _userManager;
    public GetUserInfoHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId) ?? throw new AuthenticationException("User not found");
        var isAdmin = await IsUserAdmin(user);

        return new GetUserInfoResponse(user.Avatar, user.UserName!, user.Id, user.Email!, isAdmin);
    }
    
    private async Task<bool> IsUserAdmin(AppUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        return userRoles.Any(ur => ur == "Admin");
    }
}