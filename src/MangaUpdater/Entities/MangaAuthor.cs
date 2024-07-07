using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infrastructure.Entities;

[PrimaryKey("MangaId", "Name")]
public partial class MangaAuthor
{
    [Key]
    public int MangaId { get; set; }

    [Key]
    public string Name { get; set; } = null!;

    public int Id { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("MangaAuthors")]
    public virtual Manga Manga { get; set; } = null!;
}
