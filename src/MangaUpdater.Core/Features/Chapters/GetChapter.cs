using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Chapters;

public record GetChapterQuery([FromQuery] int ChapterId) : IRequest<GetChapterResponse>;
public record GetChapterResponse(Chapter? Chapter);

public sealed class GetChapterHandler : IRequestHandler<GetChapterQuery, GetChapterResponse>
{
    private readonly AppDbContextIdentity _context;
    
    public GetChapterHandler(AppDbContextIdentity context)
    {
        _context = context;
    }

    public async Task<GetChapterResponse> Handle(GetChapterQuery request, CancellationToken cancellationToken)
    {
        var chapter = await _context.Chapters
            .AsNoTracking()
            .Where(x => x.Id == request.ChapterId)
            .SingleOrDefaultAsync(cancellationToken);

        return new GetChapterResponse(chapter);
    }
}