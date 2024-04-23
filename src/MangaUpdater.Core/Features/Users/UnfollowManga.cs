using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Users;

public record UnfollowMangaCommand([FromRoute] int MangaId) : IRequest;

public sealed class UnfollowMangaHandler : IRequestHandler<UnfollowMangaCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UnfollowMangaHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(UnfollowMangaCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        
        var userManga = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);;

        if (userManga is null) return;
        
        var userChapters = await _context.UserChapters
            .Where(uc => uc.UserMangaId == userManga.Id)
            .ToListAsync(cancellationToken);

        _context.UserChapters.RemoveRange(userChapters);
        _context.UserMangas.Remove(userManga);
        await _context.SaveChangesAsync(cancellationToken);
    }
}