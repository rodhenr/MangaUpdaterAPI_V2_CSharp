using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MangaUpdater.Application.Interfaces.Scraping;

namespace MangaUpdater.Application.Services.Scraping;

public class UpdateChaptersService : IUpdateChaptersService
{
    private readonly ChromeDriver _driver;

    public UpdateChaptersService(ChromeDriver driver)
    {
        _driver = driver;
    }

    public Dictionary<string, string> UpdateChaptersFromMangaLivreSource(string sourceUrl, string linkUrl)
    {
        var fullUrl = sourceUrl + linkUrl;

        _driver.Navigate().GoToUrl(fullUrl);

        Thread.Sleep(100);
        
        var chapters = new Dictionary<string, string>();
        var lastThreeChapters = _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]")).Take(3);

        foreach (var chapter in lastThreeChapters)
        {
            var chapterNumber = chapter.FindElement(By.ClassName("cap-text")).Text.Replace("Capítulo", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapter-date")).Text.Trim();

            chapters.Add(chapterNumber, chapterDate);
        }

        return chapters;
    }

    public Dictionary<string, string> UpdateChaptersFromAsuraScansSource(string sourceUrl, string linkUrl)
    {
        var fullUrl = sourceUrl + linkUrl;

        _driver.Navigate().GoToUrl(fullUrl);

        Thread.Sleep(100);

        var chapters = new Dictionary<string, string>();
        var lastThreeChapters = _driver.FindElements(By.CssSelector("#chapterlist ul li")).Take(3);

        foreach (var chapter in lastThreeChapters)
        {
            var chapterNumber = chapter.FindElement(By.ClassName("chapternum")).Text.Replace("Chapter", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapterdate")).Text;
            
            chapters.Add(chapterNumber, chapterDate);
        }

        return chapters;
    }
}