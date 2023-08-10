/*using MangaUpdater.Application.Interfaces;
using MangaUpdater.Domain.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MangaUpdater.Application.Services.Scraping;

public class ScrapingService : IScrapingService
{
    private readonly int _sourceId;
    private readonly string _sourceURL;
    private readonly string _linkURL;
    private readonly ChromeDriver _driver;

    public ScrapingService(int sourceId, string sourceURL, string linkURL)
    {
        _sourceId = sourceId;
        _sourceURL = sourceURL;
        _linkURL = linkURL;

        var driverOptions = new ChromeOptions()
        {
            BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
        };

        driverOptions.AddArguments(new List<string>() { "headless", "disable-gpu" });

        _driver = new ChromeDriver(driverOptions);
    }

    public MangaRegister GetMangaLivreAsync()
    {
        string fullUrl = _sourceURL + _linkURL;

        _driver.Navigate().GoToUrl(fullUrl);
        IWebElement mangaData = _driver.FindElement(By.Id("series-data"));

        string genreList = "";
        List<string> chapterList = new();
        List<string> dateList = new();

        IEnumerable<IWebElement> findChapters = _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]")).Take(3);
        string? coverURL = mangaData.FindElement(By.CssSelector(".cover img")).GetAttribute("src");
        string? name = mangaData.FindElement(By.CssSelector(".series-title h1")).Text;
        string? author = mangaData.FindElement(By.CssSelector(".series-author")).Text;
        string? synopsis = mangaData.FindElement(By.CssSelector(".series-desc span")).Text;
        string? altName = mangaData.FindElements(By.CssSelector("ol.series-synom li")).First().Text;
        var genres = mangaData.FindElements(By.CssSelector("ul.tags li a span"));

        foreach (var genre in genres)
        {
            genreList += $"{genre.Text},";
        }

        genreList = genreList.Substring(0, genreList.Length - 1);

        foreach (var url in findChapters)
        {
            chapterList.Add(url.FindElement(By.ClassName("cap-text")).Text);
            dateList.Add(url.FindElement(By.ClassName("chapter-date")).Text);
        }

        MangaRegister mangaRegister = new(coverURL, name, altName, author, synopsis, "Manga", "", _sourceId, genreList);

        return mangaRegister;
    }

    public MangaRegister GetAsuraAsync()
    {
        string fullUrl = _sourceURL + _linkURL;

        _driver.Navigate().GoToUrl(fullUrl);
        IWebElement mangaData = _driver.FindElement(By.CssSelector(".bigcontent.nobigcover"));

        string genreList = "";
        List<string> chapterList = new();
        List<string> dateList = new();
        List<string> chapters = new();
        List<DateOnly> dates = new();
        List<string> alternativeNames = new();
        List<string> tagList = new();
        List<string> authorList = new();

        IEnumerable<IWebElement> findChapters = _driver.FindElements(By.CssSelector("#chapterlist ul li")).Take(3);
        var cover = mangaData.FindElement(By.CssSelector(".thumbook img")).GetAttribute("src");
        var name = mangaData.FindElement(By.CssSelector(".entry-title")).Text;
        var author = mangaData.FindElements(By.XPath("//div[contains(@class, 'fmed')]/b[contains(text(),'Author')]/following-sibling::*"));
        var synopsis = mangaData.FindElement(By.CssSelector(".entry-content.entry-content-single")).Text;
        var altList = mangaData.FindElements(By.CssSelector(".wd-full span")).Take(1);
        var tags = mangaData.FindElements(By.CssSelector(".wd-full a"));

        foreach (var alt in altList)
        {
            alternativeNames.Add(alt.Text);
        }

        foreach (var tag in tags)
        {
            tagList.Add(tag.Text);
        }

        foreach (var aut in author)
        {
            authorList.Add(aut.Text);
        }

        foreach (var url in findChapters)
        {
            chapters.Add(url.FindElement(By.ClassName("chapternum")).Text.Replace("Chapter", "").Trim());
            dates.Add(DateOnly.Parse(url.FindElement(By.ClassName("chapterdate")).Text));
        }

        MangaRegister mangaRegister = new(coverURL, name, altName, author, synopsis, "Manga", "", _sourceId, genreList);

        return mangaRegister;
    }
}*/
