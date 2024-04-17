using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;

namespace MangaUpdater.Core.Features.Chapters;

public record UpdateChaptersQuery([FromQuery] int MangaId, [FromQuery] int ChapterId, [FromQuery] int SourceId) : IRequest<UpdateChaptersResponse>;
public record UpdateChaptersResponse();

public sealed class UpdateChaptersHandler : IRequestHandler<UpdateChaptersQuery, UpdateChaptersResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public UpdateChaptersHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<UpdateChaptersResponse> Handle(UpdateChaptersQuery request, CancellationToken cancellationToken)
    {
        var mangaSource = await _context.MangaSources
            .AsNoTracking()
            .Where(ms => ms.MangaId == request.MangaId && ms.SourceId == request.SourceId)
            .Include(ms => ms.Source)
            .SingleOrDefaultAsync(cancellationToken);
        
        var source = await _context.Sources
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.SourceId, cancellationToken);
        
        var chapters = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == request.SourceId)
            .ToListAsync(cancellationToken);

        chapters.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
                .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        var lastChapter = chapters.LastOrDefault();

        await _externalSourceService.UpdateChapters(new MangaInfoToUpdateChapters(mangaId, sourceId, mangaSource.Url,
            source.BaseUrl, source.Name, lastChapter?.Number));

        return new UpdateChaptersResponse();
    }
}