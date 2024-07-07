using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaUpdater.Infrastructure.Entities;

public partial class Genre
{
    [StringLength(20)]
    public string Name { get; set; } = null!;

    [Key]
    public int Id { get; set; }

    [InverseProperty("Genre")]
    public virtual ICollection<MangaGenre> MangaGenres { get; set; } = new List<MangaGenre>();
}