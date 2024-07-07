using MangaUpdater.Infrastructure.Entities;
using MangaUpdater.Services;
using MangaUpdater.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Features.Auth.UserInfo;

public record GetAndVerifyUserQuery(string Password) : IRequest<GetAndVerifyUserResponse>;
public record GetAndVerifyUserResponse(AppUser User);

public sealed class GetAndVerifyUserHandler : IRequestHandler<GetAndVerifyUserQuery, GetAndVerifyUserResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly CurrentUserAccessor _currentUserAccessor;
    public GetAndVerifyUserHandler(UserManager<AppUser> userManager, CurrentUserAccessor currentUserAccessor)
    {
        _userManager = userManager;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetAndVerifyUserResponse> Handle(GetAndVerifyUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(_currentUserAccessor.UserId) ?? throw new UserNotFoundException("User not found");
        
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        
        if (!isPasswordCorrect)
        {
            throw new AuthorizationException("Invalid credentials");
        }

        return new GetAndVerifyUserResponse(user);
    }
}