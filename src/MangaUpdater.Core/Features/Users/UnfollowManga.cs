using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Users;

public record UnfollowMangaQuery([FromQuery] int MangaId) : IRequest<UnfollowMangaResponse>;
public record UnfollowMangaResponse;

public sealed class UnfollowMangaHandler : IRequestHandler<UnfollowMangaQuery, UnfollowMangaResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UnfollowMangaHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<UnfollowMangaResponse> Handle(UnfollowMangaQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        
        var userManga = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);;

        if (userManga is not null)
        {
            var userChapters = await _context.UserChapters
                .Where(uc => uc.UserMangaId == userManga.Id)
                .ToListAsync(cancellationToken);

            _context.UserChapters.RemoveRange(userChapters);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var userMangas = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .ToListAsync(cancellationToken);

        _context.UserMangas.RemoveRange(userMangas);
        await _context.SaveChangesAsync(cancellationToken);

        return new UnfollowMangaResponse();
    }
}