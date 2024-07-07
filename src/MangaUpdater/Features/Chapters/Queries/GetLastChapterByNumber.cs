using System.Globalization;
using MangaUpdater.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.Chapters.GetLast;

public record GetLastChapterByNumberQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<GetLastChapterByNumberResponse>;

public record GetLastChapterByNumberResponse(float Number);

public sealed class GetLastChapterByNumberHandler : IRequestHandler<GetLastChapterByNumberQuery, GetLastChapterByNumberResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetLastChapterByNumberHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetLastChapterByNumberResponse> Handle(GetLastChapterByNumberQuery request, CancellationToken cancellationToken)
    {
        var chapters = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        chapters.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
            .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        var lastChapter = chapters.LastOrDefault();

        return new GetLastChapterByNumberResponse(float.Parse(lastChapter?.Number ?? "0", CultureInfo.InvariantCulture));
    }
}