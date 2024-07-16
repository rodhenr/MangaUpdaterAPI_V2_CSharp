using MangaUpdater.Database;
using MangaUpdater.DTOs;
using MangaUpdater.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.UserMangas.Queries;

public record GetUserMangasQuery([FromRoute] string? UserId = null, [FromQuery] int Page = 1, [FromQuery] int Limit = 20) : IRequest<List<GetUserMangasResponse>>;

public record GetUserMangasResponse(int Id, string CoverUrl, string Name, List<ChapterDto> RecentChapters);

public record BaseInfo(int MangaId, string CoverUrl, string Name, List<int> SourceIds);

public sealed class GetUserMangasHandler : IRequestHandler<GetUserMangasQuery, List<GetUserMangasResponse>>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    private readonly IMediator _mediator;
    
    public GetUserMangasHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor, IMediator mediator)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
        _mediator = mediator;
    }

    public async Task<List<GetUserMangasResponse>> Handle(GetUserMangasQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId ?? _currentUserAccessor.UserId;
        
        var maxLimit = request.Limit > 100 ? 100 : request.Limit;
        var skip = (request.Page - 1) * request.Limit;
        
        var mangas = await _context.UserMangas
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Manga.Chapters.OrderByDescending(ch => ch.Date).First().Date)
            .Skip(skip)
            .Take(maxLimit)
            .Select(x => new BaseInfo(
                x.MangaId,
                x.Manga.CoverUrl,
                x.Manga.MangaTitles.First(y => y.IsMyAnimeListMainTitle).Name,
                x.Manga.MangaSources.Select(y => y.SourceId).ToList()
            ))
            .ToListAsync(cancellationToken);

        return await GetMangaInfo(mangas, cancellationToken);
    }
    
    private async Task<List<GetUserMangasResponse>> GetMangaInfo(List<BaseInfo> mangasInfo, CancellationToken cancellationToken)
    {
        var result = new List<GetUserMangasResponse>();
        
        foreach (var manga in mangasInfo)
        {
            var recentChapters = await _mediator.Send(new GetRecentChaptersQuery(manga.MangaId, manga.SourceIds), cancellationToken);
            result.Add(new GetUserMangasResponse(manga.MangaId, manga.CoverUrl, manga.Name, recentChapters));
        }

        return result;
    }
}