using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MangaUpdater.Core.Common.Helpers;
using MangaUpdater.Core.Features.Chapters;
using MangaUpdater.Core.Features.MangaSources;
using MangaUpdater.Data;
using MangaUpdater.Data.Entities.Models;
using MediatR;

namespace MangaUpdater.Core.Features.External;

public record UpdateChaptersFromAsuraScansCommand(int MangaId, int SourceId, string SourceUrl) : IRequest;

public sealed partial class GetMangasFromAsuraScansHandler : IRequestHandler<UpdateChaptersFromAsuraScansCommand>
{
    private readonly AppDbContextIdentity _context;
    private readonly HttpClient _httpClient;
    private readonly IMediator _mediator;
    private readonly List<Chapter> _chapterList = [];

    public GetMangasFromAsuraScansHandler(AppDbContextIdentity context, IHttpClientFactory clientFactory,
        IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
        _httpClient = clientFactory.CreateClient();
    }

    public async Task Handle(UpdateChaptersFromAsuraScansCommand request, CancellationToken cancellationToken)
    {
        var mangaSource = await _mediator.Send(new GetMangaSourceQuery(request.MangaId, request.SourceId), cancellationToken);
        var lastChapterNumber = await _mediator.Send(new GetLastChapterQuery(request.MangaId, request.SourceId), cancellationToken);

        var html = await _httpClient.GetStringAsync($"{request.SourceUrl}{mangaSource.Url}", cancellationToken);

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var chapterNodes = htmlDoc.GetElementbyId("chapterlist").Descendants("li");
        ProcessApiResult(request, chapterNodes, lastChapterNumber.Number);

        _context.Chapters.AddRange(_chapterList.Distinct(new ChapterEqualityComparer()).ToList());
        await _context.SaveChangesAsync(cancellationToken);
    }

    private void ProcessApiResult(UpdateChaptersFromAsuraScansCommand request, IEnumerable<HtmlNode> nodes, float lastChapterNumber)
    {
        foreach (var chapterNode in nodes)
        {
            var chapterNumberString = chapterNode
                .SelectSingleNode(".//span[@class='chapternum']").InnerText.Replace("Chapter ", "")
                .Trim();
            var chapterDateString = chapterNode
                .SelectSingleNode(".//span[@class='chapterdate']").InnerText
                .Trim();

            var chapterNumber = ExtractNumberFromString(chapterNumberString);
            if (chapterNumber <= lastChapterNumber) break;

            var chapter = new Chapter
            {
                MangaId = request.MangaId,
                SourceId = request.SourceId,
                Number = chapterNumber.ToString(CultureInfo.InvariantCulture),
                Date = DateTime.Parse(chapterDateString)
            };

            _chapterList.Add(chapter);
        }
    }

    private static float ExtractNumberFromString(string input)
    {
        var match = MyRegex().Match(input);

        if (!match.Success) return 0;

        var numericPart = match.Groups[1].Value;

        if (float.TryParse(numericPart, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatResult))
            return floatResult;
        if (int.TryParse(numericPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intResult))
            return intResult;

        throw new InvalidOperationException("Failed to parse the numeric part as either float or int.");
    }

    [GeneratedRegex(@"(\d+(\.\d+)?)")]
    private static partial Regex MyRegex();
}