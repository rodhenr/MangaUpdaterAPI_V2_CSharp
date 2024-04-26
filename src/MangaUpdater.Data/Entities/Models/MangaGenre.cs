using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

[PrimaryKey("MangaId", "GenreId")]
[Index("GenreId", Name = "IX_MangaGenres_GenreId")]
public partial class MangaGenre
{
    [Key]
    public int MangaId { get; set; }

    [Key]
    public int GenreId { get; set; }

    public int Id { get; set; }

    [ForeignKey("GenreId")]
    [InverseProperty("MangaGenres")]
    public virtual Genre Genre { get; set; } = null!;

    [ForeignKey("MangaId")]
    [InverseProperty("MangaGenres")]
    public virtual Manga Manga { get; set; } = null!;
}
