using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using MangaUpdater.Application.Interfaces.Scraping;

namespace MangaUpdater.Application.Services.Scraping;

public class RegisterSourceService : IRegisterSourceService
{
    private readonly ChromeDriver _driver;

    public RegisterSourceService(ChromeDriver driver)
    {
        _driver = driver;
    }

    public Dictionary<string, string> RegisterFromMangaLivreSource(string sourceUrl, string linkUrl, string mangaName)
    {
        _driver.Navigate().GoToUrl(sourceUrl + linkUrl);

        var name = _driver.FindElement(By.CssSelector(".series-title h1")).Text;
        var altList = _driver.FindElements(By.CssSelector("ol.series-synom li"));

        List<string> possibleNames = new();
        Dictionary<string, string> chapters = new();

        possibleNames.Add(name);

        possibleNames.AddRange(altList.Select(alternativeName => alternativeName.Text.Replace(" ", "")));

        if (!possibleNames.Any(n =>
                string.Equals(n, mangaName.Replace(" ", ""), StringComparison.CurrentCultureIgnoreCase)))
            return chapters;

        IJavaScriptExecutor js = _driver;
        js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        Thread.Sleep(2000);

        IEnumerable<IWebElement> allChapters =
            _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]"));

        foreach (var chapter in allChapters)
        {
            var chapterNumber =
                chapter.FindElement(By.ClassName("cap-text")).Text.Replace("Capítulo", "").Trim();
            var chapterDate = chapter.FindElement(By.ClassName("chapter-date")).Text.Trim();

            chapters.Add(chapterNumber, chapterDate);
        }

        return chapters;
    }
}