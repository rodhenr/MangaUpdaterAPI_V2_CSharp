using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Mangas;

public record GetMangaFollowersCountQuery([FromRoute] int MangaId) : IRequest<GetMangaFollowersCountResponse>;

public record GetMangaFollowersCountResponse(int MangaId, int Followers);

public sealed class GetMangaFollowersCountHandler : IRequestHandler<GetMangaFollowersCountQuery, GetMangaFollowersCountResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetMangaFollowersCountHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetMangaFollowersCountResponse> Handle(GetMangaFollowersCountQuery request, CancellationToken cancellationToken)
    {
        var usersFollowing = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId)
            .CountAsync(cancellationToken);

        return new GetMangaFollowersCountResponse(request.MangaId, usersFollowing);
    }
}