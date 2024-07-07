using MangaUpdater.Exceptions;
using MangaUpdater.Services;
using MangaUpdater.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.UserMangas.Commands;

public record UpdateUserChapterCommand(int MangaId, int SourceId, int ChapterId) : IRequest;

public record UpdateChapterRequest(int ChapterId);

public sealed class UpdateChapterHandler : IRequestHandler<UpdateUserChapterCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UpdateChapterHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(UpdateUserChapterCommand request, CancellationToken cancellationToken)
    {
        _ = await _context.Chapters
            .Where(x => x.MangaId == request.MangaId && x.SourceId == request.SourceId && x.Id == request.ChapterId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException("Invalid chapter.");
        
        var userChapter = await _context.UserChapters
            .Where(x => x.SourceId == request.SourceId && x.UserManga.UserId == _currentUserAccessor.UserId && x.UserManga.MangaId == request.MangaId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException("UserChapter not found.");
        
        if (userChapter.ChapterId == request.ChapterId) return;
        
        userChapter.ChapterId = request.ChapterId;
        await _context.SaveChangesAsync(cancellationToken);
    }
}