using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.Users;

public record GetUserMangasQuery([FromQuery] string? UserId = null, [FromQuery] int Page = 1, [FromQuery] int Limit = 20) : IRequest<GetUserMangasResponse>;
public record GetUserMangasResponse(IEnumerable<UserMangaInfo> Mangas);
public record UserMangaInfo(int Id, string CoverUrl, string Name);

//public record ChapterInfo(int ChapterId, int SourceId, string SourceName, DateTime Date, string Number, bool IsUserAllowedToRead, bool Read);

public sealed class GetUserMangasHandler : IRequestHandler<GetUserMangasQuery, GetUserMangasResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public GetUserMangasHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<GetUserMangasResponse> Handle(GetUserMangasQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _currentUserAcessor.UserId;
        
        var maxLimit = request.Limit > 100 ? 100 : request.Limit;
        var skip = (request.Page - 1) * request.Limit;
        
        var result = await _context.UserMangas
            .AsNoTracking()
            .Where(um => um.UserId == userId)
            .Include(um => um.Manga)
            .ThenInclude(m => m!.MangaTitles)
            .Include(um => um.Manga)
            .ThenInclude(m => m!.Chapters!)
            .Select(um => new
            {
                UserManga = um,
                LastChapter = um.Manga!.Chapters!
                    .Where(ch => ch.MangaId == um.MangaId)
                    .OrderByDescending(ch => ch.Date)
                    .First()
            })
            .OrderByDescending(um => um.LastChapter.Date)
            .Select(um => new UserMangaInfo(um.UserManga.MangaId, um.UserManga.Manga!.CoverUrl, um.UserManga.Manga.MangaTitles!.FirstOrDefault()!.Name))
            .Skip(skip)
            .Take(maxLimit)
            .ToListAsync(cancellationToken);

        return new GetUserMangasResponse(result);
    }
}