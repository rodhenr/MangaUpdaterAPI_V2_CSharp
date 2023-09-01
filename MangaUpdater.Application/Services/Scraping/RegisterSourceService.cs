using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MangaUpdater.Application.Interfaces;
using MangaUpdater.Application.Interfaces.Scraping;
using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.Services.Scraping;

public class RegisterSourceService : IRegisterSourceService
{
    private readonly ChromeDriver _driver;
    private readonly IMangaSourceService _mangaSourceService;
    private readonly IChapterService _chapterService;

    public RegisterSourceService(ChromeDriver driver, IMangaSourceService mangaSourceService,
        IChapterService chapterService)
    {
        _mangaSourceService = mangaSourceService;
        _chapterService = chapterService;
        _driver = driver;
    }

    private async Task AddNewMangaSourceAndChapters(int mangaId, int sourceId, Dictionary<string, string> chapters,
        string sourceUrl)
    {
        var chapterList = chapters.Select(ch =>
            new Chapter
            {
                MangaId = mangaId,
                SourceId = sourceId,
                Date = DateTime.Parse(ch.Value),
                Number = float.Parse(ch.Key)
            }).ToList();

        _mangaSourceService.Add(new MangaSource { MangaId = mangaId, SourceId = sourceId, Url = sourceUrl });
        _chapterService.BulkCreate(chapterList);
        await _chapterService.SaveChanges();
    }

    public async Task RegisterFromMangaLivreSource(int mangaId, int sourceId, string sourceUrl, string linkUrl,
        string mangaName)
    {
        _driver.Navigate().GoToUrl(sourceUrl + linkUrl);

        var name = _driver.FindElement(By.CssSelector(".series-title h1")).Text;
        var altList = _driver.FindElements(By.CssSelector("ol.series-synom li"));

        List<string> possibleNames = new();

        possibleNames.Add(name);
        possibleNames.AddRange(altList.Select(alternativeName => alternativeName.Text.Replace(" ", "")));

        if (!possibleNames.Any(n =>
                string.Equals(n, mangaName.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase)))
            throw new Exception("Invalid name");

        IJavaScriptExecutor js = _driver;
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        Thread.Sleep(2000);

        IEnumerable<IWebElement> allChapters =
            _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]"));

        Dictionary<string, string> chapters = new();

        foreach (var chapter in allChapters)
        {
            var chapterNumber =
                chapter.FindElement(By.ClassName("cap-text")).Text.Replace("Capítulo", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapter-date")).Text.Trim();

            chapters.Add(chapterNumber, chapterDate);
        }

        if (chapters.Count == 0) throw new Exception("No chapters found");

        await AddNewMangaSourceAndChapters(mangaId, sourceId, chapters, sourceUrl);
    }
}