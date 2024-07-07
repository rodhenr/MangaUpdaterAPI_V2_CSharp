using MangaUpdater.Features.Auth.Queries;
using MangaUpdater.Services;
using MediatR;

namespace MangaUpdater.Features.Auth.Queries;

public record GetUserProfileInfoQuery: IRequest<GetUserProfileInfoResponse>;

public record GetUserProfileInfoResponse(string Avatar, string Name, string Id, string Email);

public sealed class GetUserProfileInfoHandler : IRequestHandler<GetUserProfileInfoQuery, GetUserProfileInfoResponse>
{
    private readonly IMediator _mediator;
    private readonly CurrentUserAccessor _currentUserAccessor;
    public GetUserProfileInfoHandler(CurrentUserAccessor currentUserAccessor, IMediator mediator)
    {
        _currentUserAccessor = currentUserAccessor;
        _mediator = mediator;
    }

    public async Task<GetUserProfileInfoResponse> Handle(GetUserProfileInfoQuery request, CancellationToken cancellationToken)
    {
        var userInfo = await _mediator.Send(new GetUserInfoQuery(_currentUserAccessor.UserId), cancellationToken);

        return new GetUserProfileInfoResponse(userInfo.Avatar, userInfo.Name, userInfo.Id, userInfo.Email);
    }
}