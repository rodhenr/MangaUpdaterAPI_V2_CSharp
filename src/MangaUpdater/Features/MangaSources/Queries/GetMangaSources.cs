using MangaUpdater.Services;
using MangaUpdater.Database;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.MangaSources.Queries;

public record GetMangaSourcesQuery([FromRoute] int MangaId) : IRequest<List<GetMangaSourcesResponse>>;

public record GetMangaSourcesResponse(int Id, string Name, bool IsUserFollowing);

public sealed class GetMangaSourcesHandler : IRequestHandler<GetMangaSourcesQuery, List<GetMangaSourcesResponse>>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetMangaSourcesHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<List<GetMangaSourcesResponse>> Handle(GetMangaSourcesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;

        var mangaSources = await _context.MangaSources
            .AsNoTracking()
            .Where(x => x.MangaId == request.MangaId)
            .Select(x => new { x.SourceId , x.Source.Name })
            .ToListAsync(cancellationToken);
        
        var userSources = await _context.UserMangas
            .Where(x => x.UserId == userId && x.MangaId == request.MangaId)
            .SelectMany(x => x.UserChapters.Select(y => y.SourceId))
            .ToListAsync(cancellationToken);

        return mangaSources
            .Select(x => new GetMangaSourcesResponse(x.SourceId, x.Name, userSources.Contains(x.SourceId)))
            .ToList();
    }
}