using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Application.Models;

[Table("MangasRegister")]
public class MangaRegister
{
    /* public MangaRegister(string coverURL, string name, string alternativeName, string author, string synopsis, string type, int myAnimeListId, int sourceId, string genres)
     {
         CoverURL = coverURL;
         Name = name;
         AlternativeName = alternativeName;
         Author = author;
         Synopsis = synopsis;
         Type = type;
         MyAnimeListId = myAnimeListId;
         SourceId = sourceId;
         Genres = genres;
     }*/

    [MaxLength(200)]
    public string CoverURL { get; set; }

    [MaxLength(200)]
    public string Name { get; set; }

    [MaxLength(200)]
    public string AlternativeName { get; set; }

    [MaxLength(50)]
    public string Author { get; set; }

    [MaxLength(2000)]
    public string Synopsis { get; set; }

    [MaxLength(20)]
    public string Type { get; set; }

    [MaxLength(200)]
    public int MyAnimeListId { get; set; }

    public int SourceId { get; set; }

    [MaxLength(50)]
    public string Genres { get; set; }
}
