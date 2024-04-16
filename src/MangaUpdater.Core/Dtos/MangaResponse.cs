namespace MangaUpdater.Core.Dtos;

public class MangaResponse
{
    public MangaResponse(int currentPage, int pageSize, int totalPages, MangasWithGenresDto data)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
        TotalPages = totalPages;
        Data = data;
    }

    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public MangasWithGenresDto Data { get; set; }
}