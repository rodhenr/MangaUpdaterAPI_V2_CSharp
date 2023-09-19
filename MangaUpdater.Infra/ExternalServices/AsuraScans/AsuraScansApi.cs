namespace MangaUpdater.Infra.Data.ExternalServices.AsuraScans;

public class AsuraScansApi
{
    private readonly IHttpClientFactory _clientFactory;

    public AsuraScansApi(IHttpClientFactory httpClientFactory)
    {
        _clientFactory = httpClientFactory;
    }

    public async Task GetChaptersAsync()
    {
        
    }
}