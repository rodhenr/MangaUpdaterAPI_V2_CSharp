using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetLastChapterQuery([FromQuery] int MangaId, [FromQuery] int SourceId) : IRequest<GetLastChapterResponse>;
public record GetLastChapterResponse(Chapter? Chapter);

public sealed class GetLastChapterHandler : IRequestHandler<GetLastChapterQuery, GetLastChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetLastChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetLastChapterResponse> Handle(GetLastChapterQuery request, CancellationToken cancellationToken)
    {
        var chapterList = await _context.Chapters
            .AsNoTracking()
            .Where(x => x.MangaId == request.MangaId && x.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        chapterList.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
            .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        return new GetLastChapterResponse(chapterList.LastOrDefault());
    }
}