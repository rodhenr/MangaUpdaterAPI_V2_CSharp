using MangaUpdater.Infrastructure;
using MangaUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Users.DeleteFollowedSource;

public record DeleteFollowedSourceCommand([FromRoute] int MangaId, [FromRoute] int SourceId) : IRequest;

public sealed class DeleteFollowedSourceHandler : IRequestHandler<DeleteFollowedSourceCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public DeleteFollowedSourceHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(DeleteFollowedSourceCommand request, CancellationToken cancellationToken)
    {
        var userChapters = await _context.UserMangas
            .Where(um => um.MangaId == request.MangaId && um.UserId == _currentUserAccessor.UserId)
            .SelectMany(x => x.UserChapters.Where(y => y.SourceId == request.SourceId))
            .SingleOrDefaultAsync(cancellationToken);
        
        if (userChapters is null) return;

        _context.UserChapters.Remove(userChapters);
        await _context.SaveChangesAsync(cancellationToken);
    }
}