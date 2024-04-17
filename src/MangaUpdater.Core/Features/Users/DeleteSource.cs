using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Users;

public record DeleteSourceQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<DeleteSourceResponse>;
public record DeleteSourceResponse;

//public record ChapterInfo(int ChapterId, int SourceId, string SourceName, DateTime Date, string Number, bool IsUserAllowedToRead, bool Read);

public sealed class DeleteSourceHandler : IRequestHandler<DeleteSourceQuery, DeleteSourceResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public DeleteSourceHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<DeleteSourceResponse> Handle(DeleteSourceQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        var userManga = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userManga is null) return new DeleteSourceResponse();
        
        var userChapters = await _context.UserChapters
            .Where(uc => uc.UserMangaId == userManga.Id && uc.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        _context.UserChapters.RemoveRange(userChapters);
        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteSourceResponse();
    }
}