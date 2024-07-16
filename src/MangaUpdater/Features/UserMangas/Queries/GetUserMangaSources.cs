using MangaUpdater.Database;
using MangaUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.UserMangas.Queries;

public record GetUserMangaSourcesQuery([FromRoute] int MangaId) : IRequest<List<GetUserMangaSourcesResponse>>;

public record GetUserMangaSourcesResponse(int SourceId, string SourceName, bool IsFollowing);

public sealed class GetUserMangaSourcesHandler : IRequestHandler<GetUserMangaSourcesQuery, List<GetUserMangaSourcesResponse>>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetUserMangaSourcesHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<List<GetUserMangaSourcesResponse>> Handle(GetUserMangaSourcesQuery request, 
        CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        
        return await _context.MangaSources
            .Where(x => x.MangaId == request.MangaId)
            .Select(x => new GetUserMangaSourcesResponse
            (
                x.SourceId,
                x.Source.Name,
                x.Manga.UserMangas.Any(y => y.UserId == userId && 
                                            y.UserChapters.Any(z => z.SourceId == x.SourceId))
            ))
            .ToListAsync(cancellationToken); 
    }
}