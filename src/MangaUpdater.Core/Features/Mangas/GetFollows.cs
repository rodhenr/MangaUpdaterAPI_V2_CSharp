using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Mangas;

public record GetFollowsQuery([FromQuery] int MangaId) : IRequest<GetFollowsResponse>;
public record GetFollowsResponse(int MangaId, int TotalFollows);

public sealed class GetFollowsHandler : IRequestHandler<GetFollowsQuery, GetFollowsResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetFollowsHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetFollowsResponse> Handle(GetFollowsQuery request, CancellationToken cancellationToken)
    {
        var usersFollowing = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId)
            .CountAsync(cancellationToken);

        return new GetFollowsResponse(request.MangaId, usersFollowing);
    }
}