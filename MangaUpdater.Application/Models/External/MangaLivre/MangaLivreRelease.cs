namespace MangaUpdater.Application.Models.External.MangaLivre;

public class MangaLivreRelease
{
    public required Dictionary<string, MangaLivreReleaseInfo> ReleaseInfo { get; set; }
}