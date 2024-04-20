using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using MangaUpdater.Core.Services;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.Mangas;

public record GetMangaQuery([FromQuery] int MangaId) : IRequest<GetMangaResponse>;
public record GetMangaResponse(Manga? Manga);

public sealed class GetMangaHandler : IRequestHandler<GetMangaQuery, GetMangaResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly CurrentUserAccessor _currentUserAccessor;
    
    public GetMangaHandler(AppDbContextIdentity context, CurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<GetMangaResponse> Handle(GetMangaQuery request, CancellationToken cancellationToken)
    {
        // I WAS RETURNING HIGHLIGHTED MANGAS HERE, I THINK I SHOULD NOT RETURN IT ANYMORE
        var userId = _currentUserAccessor.UserId;

        var m1 = _context.Mangas
            .AsNoTracking()
            .Include(m => m.UserMangas!.Where(um => um.UserId == userId))
            .ThenInclude(um => um.UserChapter!)
            .ThenInclude(uc => uc.Chapter)
            .AsQueryable();
        
        // if
        var m2 = m1.Include(m => m.MangaGenres!)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MangaSources!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaTitles)
            .SingleOrDefaultAsync(m => m.Id == request.MangaId, cancellationToken);

        var result = await m2;

        if (result != null)
        {
            result.Chapters = result.Chapters?.OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture));

        }
        
        return new GetMangaResponse(result);
    }
}