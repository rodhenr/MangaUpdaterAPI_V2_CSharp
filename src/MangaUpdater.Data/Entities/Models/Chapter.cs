using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;
[Index("MangaId", "SourceId", "Number", Name = "IX_Chapters_MangaId_SourceId_Number", IsUnique = true)]
[Index("SourceId", Name = "IX_Chapters_SourceId")]
public partial class Chapter
{
    [Key]
    public int Id { get; set; }

    public int MangaId { get; set; }

    public int SourceId { get; set; }

    public string Number { get; set; } = null!;

    public DateTime Date { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("Chapters")]
    public virtual Manga Manga { get; set; } = null!;

    [ForeignKey("SourceId")]
    [InverseProperty("Chapters")]
    public virtual Source Source { get; set; } = null!;

    [InverseProperty("Chapter")]
    public virtual ICollection<UserChapter> UserChapters { get; set; } = new List<UserChapter>();
}