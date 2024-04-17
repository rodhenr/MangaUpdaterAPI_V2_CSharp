using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Users;

public record UpdateFollowedSourcesQuery(int MangaId, List<int> SourceIds) : IRequest<UpdateFollowedSourcesResponse>;
public record UpdateFollowedSourcesResponse;

public sealed class UpdateFollowedSourcesHandler : IRequestHandler<UpdateFollowedSourcesQuery, UpdateFollowedSourcesResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public UpdateFollowedSourcesHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<UpdateFollowedSourcesResponse> Handle(UpdateFollowedSourcesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        // Check userId
        
        var userManga = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userManga is null)
        {
            _context.UserMangas.Add(new UserManga { MangaId = request.MangaId, UserId = userId! });
            await _context.SaveChangesAsync(cancellationToken);
        }

        var mangaSources = await _context.MangaSources
            .AsNoTracking()
            .Where(ms => ms.MangaId == request.MangaId)
            .Include(ms => ms.Source)
            .Include(ms => ms.Manga)
            .ToListAsync(cancellationToken);
        
        var userChapters = await _context.UserChapters
            .Where(uc => uc.UserMangaId == userManga.Id)
            .ToListAsync(cancellationToken);

        var userSources = mangaSources
            .Select(ms => new {ms.SourceId, ms.Source!.Name, IsFollowing = userChapters.Any(b => b.SourceId == ms.SourceId)})
            .ToList();

        var sourceIdListToAdd = userSources
            .Where(us => !us.IsFollowing && request.SourceIds.Contains(us.SourceId))
            .Select(us => us.SourceId)
            .ToList();

        var sourceIdListToRemove = userSources
            .Where(us => us.IsFollowing && !request.SourceIds.Contains(us.SourceId))
            .Select(us => us.SourceId)
            .ToList();

        userManga ??= await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userManga is null) return new UpdateFollowedSourcesResponse();

        foreach (var sourceId in sourceIdListToAdd)
        {
            _context.UserChapters.Add(new UserChapter { UserMangaId = userManga.Id, SourceId = sourceId });
        }

        if (sourceIdListToRemove.Count > 0)
        {
            
            var chaptersToRemove = await _context.UserChapters
                .Where(uc => uc.UserMangaId == userManga.Id && request.SourceIds.Contains(uc.SourceId))
                .ToListAsync(cancellationToken);

            _context.UserChapters.RemoveRange(chaptersToRemove);
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateFollowedSourcesResponse();
    }
}