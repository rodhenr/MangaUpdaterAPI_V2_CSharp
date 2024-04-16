using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetPreviousChapterQuery([FromQuery] int MangaId, [FromQuery] int SourceId, [FromQuery] int ChapterId) : IRequest<GetPreviousChapterResponse>;
public record GetPreviousChapterResponse(Chapter? Chapter);

public sealed class GetPreviousChapterHandler : IRequestHandler<GetPreviousChapterQuery, GetPreviousChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetPreviousChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetPreviousChapterResponse> Handle(GetPreviousChapterQuery request, CancellationToken cancellationToken)
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
        
        var result = chapterList.FirstOrDefault(ch =>
            float.Parse(ch.Number, CultureInfo.InvariantCulture) <
            float.Parse(chapterNumber, CultureInfo.InvariantCulture));

        return new GetPreviousChapterResponse(result);
    }
}