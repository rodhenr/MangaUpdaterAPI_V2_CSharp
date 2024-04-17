using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Users;

public record UpdateChapterQuery(int MangaId, int SourceId, int ChapterId) : IRequest<UpdateChapterResponse>;
public record UpdateChapterResponse;

//public record ChapterInfo(int ChapterId, int SourceId, string SourceName, DateTime Date, string Number, bool IsUserAllowedToRead, bool Read);

public sealed class UpdateChapterHandler : IRequestHandler<UpdateChapterQuery, UpdateChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public UpdateChapterHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<UpdateChapterResponse> Handle(UpdateChapterQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAcessor.UserId;
        
        var userManga = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.MangaId == request.MangaId && um.UserId == userId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (userManga is null) return new UpdateChapterResponse();

        var chapter = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.Id == request.ChapterId)
            .SingleOrDefaultAsync(cancellationToken);
        
        if (chapter is null) return new UpdateChapterResponse();

        var userChapter = await _context.UserChapters
            .Where(uc => uc.UserMangaId == userManga.Id && uc.SourceId == request.SourceId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userChapter == null) return new UpdateChapterResponse();
        
        if (userChapter.ChapterId == request.ChapterId)
        {
            var chapterList = await _context.Chapters
                .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
                .ToListAsync(cancellationToken);

            chapterList.Sort((x, y) => float.Parse(y.Number, CultureInfo.InvariantCulture)
                    .CompareTo(float.Parse(x.Number, CultureInfo.InvariantCulture)));

            var chapterNumber = await _context.Chapters
                .Where(ch => ch.Id == request.ChapterId)
                .Select(ch => ch.Number)
                .SingleOrDefaultAsync(cancellationToken) ?? "0";

            var previousChapter = chapterList
                .FirstOrDefault(ch =>
                    float.Parse(ch.Number, CultureInfo.InvariantCulture) <
                    float.Parse(chapterNumber, CultureInfo.InvariantCulture));
        
            userChapter.ChapterId = previousChapter?.Id;
        }
        else
        {
            userChapter.ChapterId = request.ChapterId;
        }
        
        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateChapterResponse();
    }
}