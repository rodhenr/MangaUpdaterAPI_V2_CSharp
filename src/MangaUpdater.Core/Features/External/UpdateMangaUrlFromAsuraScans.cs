using HtmlAgilityPack;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Core.Features.External;

public record UpdateMangaUrlFromAsuraScansCommand : IRequest;

public sealed class SearchForMangaInAsuraScansHandler : IRequestHandler<UpdateMangaUrlFromAsuraScansCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    
    public SearchForMangaInAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
    }
    
    public async Task Handle(UpdateMangaUrlFromAsuraScansCommand request, CancellationToken cancellationToken)
    {
        var mangas = await _context.Mangas
            .Where(x => x.MangaSources.Any(y => y.SourceId == (int)SourceEnum.AsuraScans))
            .Select(x => new
            {
                x.MangaTitles, 
                MangaSource = x.MangaSources.Where(y => y.SourceId == (int)SourceEnum.AsuraScans)!.SingleOrDefault()!
            })
            .ToListAsync(cancellationToken);

        foreach (var manga in mangas)
        {
            await SearchAndUpdateSourceUrl(manga.MangaTitles, manga.MangaSource, cancellationToken);
        }
    }

    private async Task SearchAndUpdateSourceUrl(IEnumerable<MangaTitle> titles, MangaSource source, CancellationToken cancellationToken)
    {
        foreach (var title in titles)
        {
            var splittedTitle = title.Name.Split(' ');
            var queryString = string.Join("+", splittedTitle);
                
            var html = await _httpClient.GetStringAsync($"https://asuratoon.com/?s={queryString}", cancellationToken);
        
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
        
            var htmlResult = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'bsx')]//a");
            var titleFound = htmlResult?.SingleOrDefault(x => x.GetAttributeValue("Title", "").StartsWith(title.Name) && x.GetAttributeValue("href", "") != source.Url);

            if (titleFound is null) continue;
            
            await UpdateMangaUrl(source, titleFound.GetAttributeValue("href", ""), cancellationToken);
            break;
        }
    }

    private async Task UpdateMangaUrl(MangaSource mangaSource, string url, CancellationToken cancellationToken)
    {
        mangaSource.Url = url.Replace("https://asuratoon.com/manga/", "");
        await _context.SaveChangesAsync(cancellationToken);
    }
}