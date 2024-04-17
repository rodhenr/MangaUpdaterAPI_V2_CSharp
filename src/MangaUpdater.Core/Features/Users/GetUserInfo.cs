using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities;

namespace MangaUpdater.Core.Features.Users;

public record GetUserInfoQuery : IRequest<GetUserInfoResponse>;
public record GetUserInfoResponse(UserInfo UserInfo);
public record UserInfo(string Avatar, string Name, string Id, string Email);

public sealed class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAcessor _currentUserAcessor;
    private readonly UserManager<AppUser> _userManager;
    public GetUserInfoHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor, UserManager<AppUser> userManager)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
        _userManager = userManager;
    }

    public async Task<GetUserInfoResponse> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user is null) throw new AuthenticationException("User not found");

        return new GetUserInfoResponse(new UserInfo(user.Avatar, user.UserName!, user.Id, user.Email!));
    }
}