using System.Globalization;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Chapters;

public record GetLastChapterQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<GetLastChapterResponse>;

public record GetLastChapterResponse(float Number);

public sealed class GetLastChapterHandler : IRequestHandler<GetLastChapterQuery, GetLastChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetLastChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetLastChapterResponse> Handle(GetLastChapterQuery request, CancellationToken cancellationToken)
    {
        var chapters = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        chapters.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
            .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        var lastChapter = chapters.LastOrDefault();

        return new GetLastChapterResponse(float.Parse(lastChapter?.Number ?? "0", CultureInfo.InvariantCulture));
    }
}