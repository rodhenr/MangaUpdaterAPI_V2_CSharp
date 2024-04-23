using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Users;

public record DeleteSourceCommand([FromRoute] int MangaId, [FromRoute] int SourceId) : IRequest;

public sealed class DeleteSourceHandler : IRequestHandler<DeleteSourceCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public DeleteSourceHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(DeleteSourceCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;
        
        var userChapters = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SelectMany(x => x.UserChapter.Where(y => y.SourceId == request.SourceId))
            .SingleOrDefaultAsync(cancellationToken);
        
        if (userChapters is null) return;

        _context.UserChapters.Remove(userChapters);
        await _context.SaveChangesAsync(cancellationToken);
    }
}