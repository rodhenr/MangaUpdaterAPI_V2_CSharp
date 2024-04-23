using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Users;

public record UpdateChapterCommand(int MangaId, int SourceId, int ChapterId) : IRequest;

public record UpdateChapterRequest(int ChapterId);

public sealed class UpdateChapterHandler : IRequestHandler<UpdateChapterCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public UpdateChapterHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.UserId;

        _ = await _context.Chapters
            .Where(x => x.MangaId == request.MangaId && x.SourceId == request.SourceId && x.Id == request.ChapterId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new BadRequestException("Invalid chapter.");
        
        var userChapter = await _context.UserChapters
            .Where(x => x.SourceId == request.SourceId && x.UserManga.UserId == userId && x.UserManga.MangaId == request.MangaId)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException("UserChapter not found.");
        
        if (userChapter.ChapterId == request.ChapterId) return;
        
        userChapter.ChapterId = request.ChapterId;
        await _context.SaveChangesAsync(cancellationToken);
    }
}