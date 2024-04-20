using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Services;
using MangaUpdater.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Core.Features.Authentication;

public record RefreshTokenQuery : IRequest<RefreshTokenResponse>;

public record RefreshTokenResponse(string AccessToken, string RefreshToken);

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenQuery, RefreshTokenResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMediator _mediator;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public RefreshTokenHandler(UserManager<AppUser> userManager, IMediator mediator, CurrentUserAccessor currentUserAccessor)
    {
        _userManager = userManager;
        _mediator = mediator;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException("User not found");

        return await GetRefreshTokenInfo(user, cancellationToken);
    }

    private async Task<RefreshTokenResponse> GetRefreshTokenInfo(AppUser user, CancellationToken cancellationToken)
    {
        var tokens = await _mediator.Send(new GenerateTokenQuery(user), cancellationToken);
                
        return new RefreshTokenResponse(tokens.AccessToken, tokens.RefreshToken);
    }
}