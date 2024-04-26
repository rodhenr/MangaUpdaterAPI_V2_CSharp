using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Data.Entities.Models;

public partial class MangaTitle
{
    public int MangaId { get; set; }

    public string Name { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    public bool IsMainTitle { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("MangaTitles")]
    public virtual Manga Manga { get; set; } = null!;
}
