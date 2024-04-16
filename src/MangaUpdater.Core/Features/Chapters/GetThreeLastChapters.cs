using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetThreeLastChaptersQuery([FromQuery] int MangaId, [FromQuery] List<int> SourceList) : IRequest<GetThreeLastChaptersResponse>;
public record GetThreeLastChaptersResponse(List<Chapter> Chapters);

public sealed class GetThreeLastChaptersHandler : IRequestHandler<GetThreeLastChaptersQuery, GetThreeLastChaptersResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetThreeLastChaptersHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetThreeLastChaptersResponse> Handle(GetThreeLastChaptersQuery request, CancellationToken cancellationToken)
    {
        var chapterList = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && request.SourceList.Contains(ch.SourceId))
            .Include(ch => ch.Source)
            .OrderByDescending(ch => ch.Date)
            .ThenByDescending(ch => ch.Number)
            .Take(3)
            .ToListAsync(cancellationToken);
        
        return new GetThreeLastChaptersResponse(chapterList);
    }
}