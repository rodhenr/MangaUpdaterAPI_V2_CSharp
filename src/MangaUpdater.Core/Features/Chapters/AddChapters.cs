using MangaUpdater.Core.Common.Helpers;
using MangaUpdater.Core.Services;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MangaUpdater.Data;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Chapters;

public record AddChaptersQuery([FromQuery] int MangaId, int SourceId) : IRequest<AddChaptersResponse>;
public record AddChaptersResponse();

public sealed class AddChaptersHandler : IRequestHandler<AddChaptersQuery, AddChaptersResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly IAsuraScansApi _asuraService;
    private readonly IMangaDexApi _mangaDexService;
    
    public AddChaptersHandler(AppDbContextIdentity context, IAsuraScansApi asuraService, IMangaDexApi mangaDexService)
    {
        _context = context;
        _asuraService = asuraService;
        _mangaDexService = mangaDexService;
    }

    public async Task<AddChaptersResponse> Handle(AddChaptersQuery request, CancellationToken cancellationToken)
    {
        var mangaInfo = await _context.Mangas
            .Where(x => x.Id == request.MangaId)
            .FirstOrDefaultAsync(cancellationToken);
        
        switch (mangaInfo.MangaSources.)
        {
            case "MangaDex":
            {
                var chapters = await _mangaDexService.GetChaptersAsync(request.MangaId, request.SourceId,
                    request.MangaUrl, request.SourceBaseUrl, request.LastSavedChapter);

                _context.Chapters.AddRange(chapters.Distinct(new ChapterEqualityComparer()).ToList());
                break;
            }
            case "AsuraScans":
            {
                var chapters = await _asuraService.GetChaptersAsync(request.MangaId, request.SourceId,
                    request.MangaUrl, request.SourceBaseUrl, request.LastSavedChapter);

                _context.Chapters.AddRange(chapters.Distinct(new ChapterEqualityComparer()).ToList());
                break;
            }
        }
        
        GC.Collect();

        await _context.SaveChangesAsync(cancellationToken);

        return new AddChaptersResponse();
    }
}