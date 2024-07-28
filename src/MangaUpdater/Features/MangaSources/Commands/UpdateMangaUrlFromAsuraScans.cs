using HtmlAgilityPack;
using MangaUpdater.Database;
using MangaUpdater.Entities;
using MangaUpdater.Enums;
using MangaUpdater.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Features.MangaSources.Commands;

public record UpdateMangaUrlFromAsuraScansCommand : IRequest;

public sealed class SearchForMangaInAsuraScansHandler : IRequestHandler<UpdateMangaUrlFromAsuraScansCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    
    public SearchForMangaInAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _httpClient = clientFactory.CreateClient();
    }
    
    public async Task Handle(UpdateMangaUrlFromAsuraScansCommand request, CancellationToken cancellationToken)
    {
        var asuraScansSource = await _context.Sources
            .Where(x => x.Id == (int)MangaSourcesEnum.AsuraScans)
            .SingleOrDefaultAsync(cancellationToken) ?? throw new EntityNotFoundException("Source not found.");
        
        var mangas = await _context.Mangas
            .Where(x => x.MangaSources.Any(y => y.SourceId == (int)MangaSourcesEnum.AsuraScans) 
                && x.MangaTitles.Any(y => y.IsAsuraMainTitle))
            .Select(x => new
            {
                Title = x.MangaTitles.First(y => y.IsAsuraMainTitle == true), 
                Source = x.MangaSources.First(y => y.SourceId == (int)MangaSourcesEnum.AsuraScans)
            })
            .ToListAsync(cancellationToken);
        
        foreach (var manga in mangas)
        {
            await SearchAndUpdateSourceUrl(asuraScansSource.BaseUrl, manga.Title, manga.Source, cancellationToken);
        }
    }

    private async Task SearchAndUpdateSourceUrl(string baseUrl, MangaTitle title, MangaSource source, CancellationToken cancellationToken)
    {
        var splittedTitle = title.Name.Split(' ');
        var queryString = $"{baseUrl.Replace("manga/", "")}?name={string.Join("+", splittedTitle)}";
            
        var html = await _httpClient.GetStringAsync(queryString, cancellationToken);
    
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
            
        var newUrl = htmlDoc.DocumentNode
            .Descendants("div")
            .FirstOrDefault(div =>
                div.Descendants("span").Any(span => span.InnerText.Contains("Chapter")) &&
                div.Descendants("span").Any(span => span.InnerText.StartsWith(title.Name))
            )?
            .Descendants("a")
            .FirstOrDefault(a => a.GetAttributeValue("href", "").StartsWith("series/"))?
            .GetAttributeValue("href", "")?
            .Replace("series/", "");

        if (newUrl is null) return;
        
        await UpdateMangaUrl(baseUrl, source, newUrl, cancellationToken);
    }

    private async Task UpdateMangaUrl(string baseUrl, MangaSource mangaSource, string url, CancellationToken cancellationToken)
    {
        mangaSource.Url = url.Replace($"{baseUrl}", "");
        await _context.SaveChangesAsync(cancellationToken);
    }
}