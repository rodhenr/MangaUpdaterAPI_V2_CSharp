using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetChapterByMangaIdQuery([FromQuery] int ChapterId, [FromQuery] int MangaId) : IRequest<GetChapterByMangaIdResponse>;

public record GetChapterByMangaIdResponse(Chapter? Chapter);

public sealed class GetChapterByMangaIdHandler : IRequestHandler<GetChapterByMangaIdQuery, GetChapterByMangaIdResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetChapterByMangaIdHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetChapterByMangaIdResponse> Handle(GetChapterByMangaIdQuery request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters
            .AsNoTracking()
            .Where(x => x.Id == request.ChapterId && x.MangaId == request.MangaId)
            .SingleOrDefaultAsync(cancellationToken);

        return new GetChapterByMangaIdResponse(chapter);
    }
}