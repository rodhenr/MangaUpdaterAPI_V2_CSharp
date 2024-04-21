using System.Globalization;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Chapters;

public record GetLastChapterNumberQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<GetLastChapterNumberResponse>;
public record GetLastChapterNumberResponse(float Number);

public sealed class GetLastChapterNumberHandler : IRequestHandler<GetLastChapterNumberQuery, GetLastChapterNumberResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetLastChapterNumberHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetLastChapterNumberResponse> Handle(GetLastChapterNumberQuery request, CancellationToken cancellationToken)
    {
        var chapters = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        chapters.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
            .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        var lastChapter = chapters.LastOrDefault();

        return new GetLastChapterNumberResponse(float.Parse(lastChapter?.Number ?? "0", CultureInfo.InvariantCulture));
    }
}