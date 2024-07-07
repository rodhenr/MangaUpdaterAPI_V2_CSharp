using MangaUpdater.Infrastructure;
using MangaUpdater.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Chapters.Get;

public record GetChapterQuery([FromRoute] int MangaId, [FromRoute] int ChapterId) : IRequest<GetChapterResponse>;

public record GetChapterResponse(int Id, int MangaId, int SourceId, DateTime Date, string Number);

public sealed class GetChapterHandler : IRequestHandler<GetChapterQuery, GetChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetChapterResponse> Handle(GetChapterQuery request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters
            .AsNoTracking()
            .Where(x => x.Id == request.ChapterId && x.MangaId == request.MangaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (chapter is null)
        {
            throw new EntityNotFoundException($"Chapter not found for MangaId {request.MangaId} and ChapterId {request.ChapterId}");
        }

        return new GetChapterResponse(chapter.Id, chapter.MangaId, chapter.SourceId, chapter.Date, chapter.Number);
    }
}