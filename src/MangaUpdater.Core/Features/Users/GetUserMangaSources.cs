using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Users;

public record GetUserMangaSourcesQuery([FromQuery] int MangaId) : IRequest<GetUserMangaSourcesResponse>;
public record GetUserMangaSourcesResponse(List<UserMangaSources> Sources);

public record UserMangaSources(int SourceId, string SourceName, bool IsFollowing);

//public record ChapterInfo(int ChapterId, int SourceId, string SourceName, DateTime Date, string Number, bool IsUserAllowedToRead, bool Read);

public sealed class GetUserMangaSourcesHandler : IRequestHandler<GetUserMangaSourcesQuery, GetUserMangaSourcesResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetUserMangaSourcesHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetUserMangaSourcesResponse> Handle(GetUserMangaSourcesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        
        // TODO: Refactor
        var userManga = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
        
        var mangaSources = await _context.MangaSources
            .AsNoTracking()
            .Where(ms => ms.MangaId == request.MangaId)
            .Include(ms => ms.Source)
            .Include(ms => ms.Manga)
            .ToListAsync(cancellationToken);
        
        var userChapters = await _context.UserChapters
            .AsNoTracking()
            .Where(uc => uc.Id == userManga.Id)
            .ToListAsync(cancellationToken);

        var result =  mangaSources
            .Select(ms => new UserMangaSources(ms.SourceId, ms.Source!.Name, userChapters.Any(b => b.SourceId == ms.SourceId)))
            .ToList();

        return new GetUserMangaSourcesResponse(result);
    }
}