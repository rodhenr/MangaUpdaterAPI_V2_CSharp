using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetSmallestChapterQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<GetSmallestChapterResponse>;
public record GetSmallestChapterResponse(Chapter? Chapter);

public sealed class GetSmallestChapterHandler : IRequestHandler<GetSmallestChapterQuery, GetSmallestChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetSmallestChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetSmallestChapterResponse> Handle(GetSmallestChapterQuery request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
            .OrderBy(ch => ch.Number)
            .FirstOrDefaultAsync(cancellationToken);

        return new GetSmallestChapterResponse(chapter);
    }
}