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
    private readonly CurrentUserAcessor _currentUserAcessor;
    
    public GetMangaHandler(AppDbContextIdentity context, CurrentUserAcessor currentUserAcessor)
    {
        _context = context;
        _currentUserAcessor = currentUserAcessor;
    }

    public async Task<GetMangaResponse> Handle(GetMangaQuery request, CancellationToken cancellationToken)
    {
        // I WAS RETURNING HIGHLIGHTED MANGAS HERE, I THINK I SHOULD NOT RETURN IT ANYMORE
        var userId = _currentUserAcessor.UserId;

        var m1 = _context.Mangas
            .AsNoTracking()
            .Include(m => m.UserMangas!.Where(um => um.UserId == userId))
            .ThenInclude(um => um.UserChapter!)
            .ThenInclude(uc => uc.Chapter)
            .AsQueryable();
        
        var result = m1.Include(m => m.MangaGenres!)
            .ThenInclude(mg => mg.Genre)
            .Include(m => m.MangaSources!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.Chapters!)
            .ThenInclude(ms => ms.Source)
            .Include(m => m.MangaAuthors)
            .Include(m => m.MangaTitles)
            .SingleOrDefaultAsync(m => m.Id == request.MangaId, cancellationToken);

        if (result is not null)
            result.Chapters =
                manga.Chapters?.OrderByDescending(ch => float.Parse(ch.Number, CultureInfo.InvariantCulture));

        return new GetMangaResponse(result);
    }
}