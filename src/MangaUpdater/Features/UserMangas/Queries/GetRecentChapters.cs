using MangaUpdater.Database;
using MangaUpdater.DTOs;
using MangaUpdater.Helpers;
using MangaUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.UserMangas.Queries;

public record GetRecentChaptersQuery([FromQuery] int MangaId, [FromQuery] List<int> SourceList) 
    : IRequest<List<ChapterDto>>;

public sealed class GetRecentChaptersHandler : IRequestHandler<GetRecentChaptersQuery, List<ChapterDto>>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetRecentChaptersHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<List<ChapterDto>> Handle(GetRecentChaptersQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserAccessor.IsLoggedIn ? _currentUserAccessor.UserId : null;
        
        var chapterList = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && request.SourceList.Contains(ch.SourceId))
            .Include(ch => ch.Source)
            .OrderByDescending(ch => ch.Date)
            .ThenByDescending(ch => ch.Number)
            .Take(3)
            .ToListAsync(cancellationToken);

        if (userId is null)
        {
            return chapterList
                .Select(x => new ChapterDto(x.Id, x.MangaId, x.Source.Name, x.Date, x.Number))
                .ToList();
        }

        var userMangas = await _context.UserMangas
            .Where(x => x.MangaId == request.MangaId && x.UserId == userId)
            .Include(x => x.UserChapters)
            .ThenInclude(x => x.Chapter)
            .ToListAsync(cancellationToken);

        var userMangaInfo = userMangas
            .SelectMany(x => x.UserChapters, (userManga, userChapter) => 
                new UserMangaDto(userManga.MangaId, userChapter.SourceId,userChapter.ChapterId, userChapter.Chapter?.Number))
            .ToList();

        return UserMangaChapterInfo.GetUserChaptersState(chapterList, userMangaInfo).ToList();
    }
}