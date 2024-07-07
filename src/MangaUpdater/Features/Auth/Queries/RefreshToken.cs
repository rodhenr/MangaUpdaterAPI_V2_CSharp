using MangaUpdater.Infrastructure.Entities;
using MangaUpdater.Services;
using MangaUpdater.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaUpdater.Features.Auth.RefreshToken;

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
        var user = await _userManager.FindByIdAsync(_currentUserAccessor.UserId) 
                   ?? throw new UserNotFoundException("User not found");

        return await GetRefreshTokenInfo(user, cancellationToken);
    }

    private async Task<RefreshTokenResponse> GetRefreshTokenInfo(AppUser user, CancellationToken cancellationToken)
    {
        var tokens = await _mediator.Send(new GenerateTokenQuery(user), cancellationToken);
                
        return new RefreshTokenResponse(tokens.AccessToken, tokens.RefreshToken);
    }
}