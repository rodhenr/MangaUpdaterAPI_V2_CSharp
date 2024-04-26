using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

[PrimaryKey("MangaId", "SourceId")]
[Index("SourceId", Name = "IX_MangaSources_SourceId")]
public partial class MangaSource
{
    [Key]
    public int MangaId { get; set; }

    [Key]
    public int SourceId { get; set; }

    [StringLength(100)]
    public string Url { get; set; } = null!;

    public int Id { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("MangaSources")]
    public virtual Manga Manga { get; set; } = null!;

    [ForeignKey("SourceId")]
    [InverseProperty("MangaSources")]
    public virtual Source Source { get; set; } = null!;
}
