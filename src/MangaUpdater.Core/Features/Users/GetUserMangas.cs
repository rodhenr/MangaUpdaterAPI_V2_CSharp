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
        
        return await _context.UserMangas
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Manga.Chapters.OrderByDescending(ch => ch.Date).First().Date)
            .Skip(skip)
            .Take(maxLimit)
            .Select(x => new GetUserMangasResponse(x.MangaId, x.Manga.CoverUrl, x.Manga.MangaTitles.First().Name))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}