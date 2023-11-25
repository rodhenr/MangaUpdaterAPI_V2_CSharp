using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using MangaUpdater.Application.Interfaces.External.AsuraScans;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Infra.Data.ExternalServices.AsuraScans;

public partial class AsuraScansApiService : IAsuraScansApi
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly List<Chapter> _list = new();

    public AsuraScansApiService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<List<Chapter>> GetChaptersAsync(int mangaId, int sourceId, string mangaUrl, string sourceUrl,
        string? initialChapter)
    {
        var htmlDoc = new HtmlDocument();
        var httpClient = _clientFactory.CreateClient();

        var html = await httpClient.GetStringAsync($"{sourceUrl}{mangaUrl}");
        htmlDoc.LoadHtml(html);

        var chapterNodes = htmlDoc.GetElementbyId("chapterlist").Descendants("li");

        foreach (var chapterNode in chapterNodes)
        {
            var chapterNumberString = chapterNode.SelectSingleNode(".//span[@class='chapternum']").InnerText
                .Replace("Chapter ", "").Trim();
            var chapterDateString = chapterNode.SelectSingleNode(".//span[@class='chapterdate']").InnerText.Trim();

            var chapterNumber = ExtractNumberFromString(chapterNumberString);

            if (float.Parse(chapterNumber, CultureInfo.InvariantCulture) <=
                float.Parse(initialChapter ?? "0", CultureInfo.InvariantCulture)) break;

            var chapter = new Chapter
            {
                MangaId = mangaId,
                SourceId = sourceId,
                Number = chapterNumber,
                Date = DateTime.Parse(chapterDateString)
            };

            _list.Add(chapter);
        }

        return _list;
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

    [GeneratedRegex(@"(\d+(\.\d+)?)")]
    private static partial Regex MyRegex();
}