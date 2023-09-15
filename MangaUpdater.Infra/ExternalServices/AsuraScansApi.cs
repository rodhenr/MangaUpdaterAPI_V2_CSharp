namespace MangaUpdater.Infra.Data.ExternalServices;

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