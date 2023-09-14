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
        string linkUrl)
    {
        var chapterList = chapters.Select(ch =>
            new Chapter
            {
                MangaId = mangaId,
                SourceId = sourceId,
                Date = DateTime.Parse(ch.Value),
                Number = float.Parse(ch.Key)
            }).ToList();

        _mangaSourceService.Add(new MangaSource { MangaId = mangaId, SourceId = sourceId, Url = linkUrl });
        _chapterService.BulkCreate(chapterList);
        await _chapterService.SaveChanges();
    }

    public async Task RegisterFromMangaLivreSource(int mangaId, int sourceId, string sourceUrl, string linkUrl,
        IEnumerable<MangaTitle> mangaTitles)
    {
        _driver.Navigate().GoToUrl(sourceUrl + linkUrl);

        var mangaName = _driver.FindElement(By.CssSelector(".series-title h1")).Text;

        if (mangaTitles.All(mt => mt.Name != mangaName))
            throw new Exception("Invalid name");

        IJavaScriptExecutor js = _driver;
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        Thread.Sleep(2000);

        var allChapters = _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]"));

        var chapters = new Dictionary<string, string>();

        foreach (var chapter in allChapters)
        {
            var chapterNumber = chapter.FindElement(By.ClassName("cap-text")).Text.Replace("Capítulo", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapter-date")).Text.Trim();

            chapters.Add(chapterNumber, chapterDate);
        }

        if (chapters.Count == 0) throw new Exception("No chapters found");

        await AddNewMangaSourceAndChapters(mangaId, sourceId, chapters, linkUrl);
    }

    public async Task RegisterFromAsuraScansSource(int mangaId, int sourceId, string sourceUrl, string linkUrl,
        IEnumerable<MangaTitle> mangaTitles)
    {
        _driver.Navigate().GoToUrl(sourceUrl + linkUrl);

        var mangaName = _driver.FindElement(By.CssSelector(".entry-title")).Text;

        if (mangaTitles.All(mt => mt.Name != mangaName))
            throw new Exception("Invalid name");

        var allChapters = _driver.FindElements(By.CssSelector(".eph-num"));

        var chapters = new Dictionary<string, string>();

        foreach (var chapter in allChapters)
        {
            var chapterNumber = chapter.FindElement(By.ClassName("chapternum")).Text.Replace("Chapter", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapterdate")).Text;

            chapters.Add(chapterNumber, chapterDate);
        }

        if (chapters.Count == 0) throw new Exception("No chapters found");

        await AddNewMangaSourceAndChapters(mangaId, sourceId, chapters, sourceUrl);
    }
}