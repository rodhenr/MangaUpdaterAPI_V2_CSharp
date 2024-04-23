using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Users;

public record GetUserMangasQuery([FromRoute] string? UserId = null, [FromQuery] int Page = 1, [FromQuery] int Limit = 20) : IRequest<List<GetUserMangasResponse>>;

public record GetUserMangasResponse(int Id, string CoverUrl, string Name);

public sealed class GetUserMangasHandler : IRequestHandler<GetUserMangasQuery, List<GetUserMangasResponse>>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetUserMangasHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<List<GetUserMangasResponse>> Handle(GetUserMangasQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _currentUserAccessor.UserId;
        
        var maxLimit = request.Limit > 100 ? 100 : request.Limit;
        var skip = (request.Page - 1) * request.Limit;
        
        var result = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.UserId == userId)
            .Include(um => um.Manga)
            .ThenInclude(m => m.MangaTitles)
            .Include(um => um.Manga)
            .ThenInclude(m => m.Chapters)
            .Select(um => new
            {
                UserManga = um,
                LastChapter = um.Manga.Chapters
                    .Where(ch => ch.MangaId == um.MangaId)
                    .OrderByDescending(ch => ch.Date)
                    .First()
            })
            .OrderByDescending(um => um.LastChapter.Date)
            .Skip(skip)
            .Take(maxLimit)
            .Select(um => new GetUserMangasResponse(um.UserManga.MangaId, um.UserManga.Manga.CoverUrl, um.UserManga.Manga.MangaTitles.FirstOrDefault()!.Name))
            .ToListAsync(cancellationToken);

        return result;
    }
}