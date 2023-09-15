namespace MangaUpdater.Application.Models;

public class MangaLivreRelease
{
    public required Dictionary<string, MangaLivreReleaseInfo> ReleaseInfo { get; set; }
}