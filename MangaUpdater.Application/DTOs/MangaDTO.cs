using MangaUpdater.Domain.Entities;

namespace MangaUpdater.Application.DTOs;

public class MangaDTO
{ 
    public string CoverURL { get; set; }
    
    public string Name { get; set; }
    
    public string AlternativeName { get; set; }
    
    public string Author { get; set; }
   
    public string Synopsis { get; set; }

    public string Type { get; set; }
    
    public string MyAnimeListURL { get; set; }

    public bool IsUserFollowing { get; set; }
    public IEnumerable<string> Genres { get; set; }

    public IEnumerable<SourceDTO> Sources { get; set; }

   public IEnumerable<ChapterDTO> Chapters { get; set; }
}
