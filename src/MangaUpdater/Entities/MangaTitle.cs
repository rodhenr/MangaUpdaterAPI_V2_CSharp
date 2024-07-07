using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Entities;

public partial class MangaTitle
{
    public int MangaId { get; set; }

    public string Name { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    public bool IsMyAnimeListMainTitle { get; set; }
    
    public bool IsAsuraMainTitle { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("MangaTitles")]
    public virtual Manga Manga { get; set; } = null!;
}
