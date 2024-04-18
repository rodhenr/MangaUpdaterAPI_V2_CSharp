using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using HtmlAgilityPack;
using MediatR;
using MangaUpdater.Core.Common.Helpers;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;

namespace MangaUpdater.Core.Features.External;

public record UpdateChaptersFromAsuraScansQuery(int MangaId) : IRequest<GetMangasFromAsuraScansResponse>;
public record GetMangasFromAsuraScansResponse;

public sealed class GetMangasFromAsuraScansHandler : IRequestHandler<UpdateChaptersFromAsuraScansQuery, GetMangasFromAsuraScansResponse>
{
    private readonly AppDbContextIdentity _context;
    private readonly IHttpClientFactory _clientFactory;
    
    public GetMangasFromAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory)
    {
        _context = context;
        _clientFactory = clientFactory;
    }

    public async Task<GetMangasFromAsuraScansResponse> Handle(UpdateChaptersFromAsuraScansQuery request, CancellationToken cancellationToken)
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
        
        using var httpClient = _clientFactory.CreateClient();
        var html = await httpClient.GetStringAsync($"{source.BaseUrl}{manga.Url}", cancellationToken);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);
        
        var chapterNodes = htmlDoc.GetElementbyId("chapterlist").Descendants("li");
        
        foreach (var chapterNode in chapterNodes)
        {
            var chapterNumberString = chapterNode.SelectSingleNode(".//span[@class='chapternum']").InnerText.Replace("Chapter ", "").Trim();
            var chapterDateString = chapterNode.SelectSingleNode(".//span[@class='chapterdate']").InnerText.Trim();

            var chapterNumber = ExtractNumberFromString(chapterNumberString);

            if (float.Parse(chapterNumber, CultureInfo.InvariantCulture) <=
                float.Parse(lastChapter?.Number ?? "0", CultureInfo.InvariantCulture)) break;

            var chapter = new Chapter
            {
                MangaId = request.MangaId,
                SourceId = source.Id,
                Number = chapterNumber,
                Date = DateTime.Parse(chapterDateString)
            };

            chaptersToCreate.Add(chapter);
        }
        
        _context.Chapters.AddRange(chaptersToCreate.Distinct(new ChapterEqualityComparer()).ToList());
        
        await _context.SaveChangesAsync(cancellationToken);

        return new GetMangasFromAsuraScansResponse();
    }

    private static string ExtractNumberFromString(string input)
    {
        var match = MyRegex().Match(input);

        if (!match.Success) return "0";

        var numericPart = match.Groups[1].Value;

        if (float.TryParse(numericPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatResult))
        {
            return floatResult.ToString(CultureInfo.InvariantCulture);
        }

        if (int.TryParse(numericPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intResult))
        {
            return intResult.ToString(CultureInfo.InvariantCulture);
        }

        throw new InvalidOperationException("Failed to parse the numeric part as either float or int.");
    }

    private static Regex MyRegex()
    {
        return new Regex(@"(\d+(\.\d+)?)");
    }
}