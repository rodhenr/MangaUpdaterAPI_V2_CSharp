using System.Globalization;
using MangaUpdater.Core.Common.Exceptions;
using MangaUpdater.Core.Common.Extensions;
using MediatR;
using MangaUpdater.Core.Common.Helpers;
using MangaUpdater.Core.Models;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.External;

public record UpdateChaptersFromMangaDexQuery(int MangaId) : IRequest<GetMangasFromMangaDexResponse>;
public record GetMangasFromMangaDexResponse;

public sealed class GetMangasFromMangaDexHandler : IRequestHandler<UpdateChaptersFromMangaDexQuery, GetMangasFromMangaDexResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly IHttpClientFactory _clientFactory;
    
    public GetMangasFromMangaDexHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _clientFactory = clientFactory;
    }

    public async Task<GetMangasFromMangaDexResponse> Handle(UpdateChaptersFromMangaDexQuery request, CancellationToken cancellationToken)
    {
        var source = await _context.Sources.FirstOrDefaultAsync(x => x.Name == "MangaDex", cancellationToken);

        if (source is null) throw new Exception("Source not found");
        
        var manga = await _context.MangaSources.FirstOrDefaultAsync(x => x.MangaId == request.MangaId && x.SourceId == source.Id, cancellationToken);
        
        if (manga is null) throw new Exception("Manga not found");
        
        var chapters = await _context.Chapters
            .AsNoTracking()
            .Where(ch => ch.MangaId == request.MangaId && ch.SourceId == source.Id)
            .ToListAsync(cancellationToken);

        chapters.Sort((x, y) => float.Parse(x.Number, CultureInfo.InvariantCulture)
            .CompareTo(float.Parse(y.Number, CultureInfo.InvariantCulture)));

        var lastChapter = chapters.LastOrDefault();
        
        var chaptersToCreate = new List<Chapter>();
        
        var httpClient = _clientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", "MangaUpdater/1.0");

        var offset = 0;

        while (true)
        {
            var options = $"feed?translatedLanguage[]=en&limit=199&order[chapter]=asc&limit=500&offset={offset}";
            var url = $"{source.BaseUrl}{manga.Url}/{options}";

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode) throw new BadRequestException($"Failed to retrieve data for ID `{manga.Url}` from MangaDex");

            var content = await response.Content.TryToReadJsonAsync<MangaDexModel>();

            if (content is null || content.Data.Count == 0) break;

            foreach (var chapter in content.Data)
            {
                var chapterNumber = float.Parse(chapter.Attributes.Chapter, CultureInfo.InvariantCulture);

                if (chapterNumber <= float.Parse(lastChapter?.Number ?? "0", CultureInfo.InvariantCulture)) continue;

                chaptersToCreate.Add(new Chapter
                {
                    MangaId = request.MangaId,
                    SourceId = source.Id,
                    Number = chapter.Attributes.Chapter,
                    Date = DateTime.Parse(chapter.Attributes.CreatedAt)
                });
            }

            offset += 200;
        }
        
        _context.Chapters.AddRange(chaptersToCreate.Distinct(new ChapterEqualityComparer()).ToList());
        
        await _context.SaveChangesAsync(cancellationToken);

        return new GetMangasFromMangaDexResponse();
    }
}