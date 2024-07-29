using MangaUpdater.Entities;
using MangaUpdater.Exceptions;
using MangaUpdater.Features.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Features.Auth.Queries;

public record AuthenticateUserQuery(string Email, string Password) : IRequest<AuthenticateUserResponse>;

public record AuthenticateUserResponse(string UserName, string UserAvatar, string AccessToken, string RefreshToken, bool IsAdmin);

public sealed class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, AuthenticateUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMediator _mediator;
    
    public AuthenticateUserHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMediator mediator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _mediator = mediator;
    }

    public async Task<AuthenticateUserResponse> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserNotFoundException("User not found.");
        }

        var result = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, false, false);

        if (result.Succeeded)
        {
            return await GetAuthenticateUserInfo(user, cancellationToken);
        }

        throw new BadRequestException(GetErrorMessage(result));
    }

    private async Task<AuthenticateUserResponse> GetAuthenticateUserInfo(AppUser user, CancellationToken cancellationToken)
    {
        var tokens = await _mediator.Send(new GenerateTokenQuery(user), cancellationToken);
        var userInfo = await _mediator.Send(new GetUserInfoQuery(user.Id), cancellationToken);
                
        return new AuthenticateUserResponse(userInfo.Name, userInfo.Avatar, tokens.AccessToken, tokens.RefreshToken, userInfo.IsAdmin);
    }

    private static string GetErrorMessage(SignInResult result)
    {
        return result switch
        {
            _ when result.IsLockedOut => "Account blocked",
            _ when result.IsNotAllowed => "Not allowed",
            _ when result.RequiresTwoFactor => "Require two factor",
            _ => "Invalid user/password"
        };
    }
}