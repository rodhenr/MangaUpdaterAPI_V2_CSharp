using MangaUpdater.Application.Interfaces.Scraping;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MangaUpdater.Application.Services.Scraping;

public class UpdateChaptersService : IUpdateChaptersService
{
    private readonly ChromeDriver _driver;

    public UpdateChaptersService(ChromeDriver driver)
    {
        _driver = driver;
    }

    public async Task UpdateChaptersFromMangaLivreSource(string sourceUrl, string linkUrl)
    {
        string fullUrl = sourceUrl + linkUrl;

        _driver.Navigate().GoToUrl(fullUrl);

        Dictionary<string, string> chapters = new();

        IEnumerable<IWebElement> lastThreeChapters = _driver.FindElements(By.XPath("//li[a[contains(@class, 'link-dark')]]")).Take(3);

        foreach (var chapter in lastThreeChapters)
        {
            string chapterNumber = chapter.FindElement(By.ClassName("cap-text")).Text.Replace("Capítulo", "").Trim();
            string chapterDate = chapter.FindElement(By.ClassName("chapter-date")).Text.Trim();

            chapters.Add(chapterNumber, chapterDate);
        }

        return;
    }

    /*  public async Task<MangaRegister> GetAsuraAsync()
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
      }*/
}
