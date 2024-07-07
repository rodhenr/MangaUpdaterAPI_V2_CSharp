using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infrastructure.Entities;

[Index("ChapterId", Name = "IX_UserChapters_ChapterId")]
[Index("SourceId", Name = "IX_UserChapters_SourceId")]
[Index("UserMangaId", "SourceId", Name = "IX_UserChapters_UserMangaId_SourceId", IsUnique = true)]
public partial class UserChapter
{
    [Key]
    public int Id { get; set; }

    public int UserMangaId { get; set; }

    public int? ChapterId { get; set; }

    public int SourceId { get; set; }

    [ForeignKey("ChapterId")]
    [InverseProperty("UserChapters")]
    public virtual Chapter? Chapter { get; set; }

    [ForeignKey("SourceId")]
    [InverseProperty("UserChapters")]
    public virtual Source Source { get; set; } = null!;

    [ForeignKey("UserMangaId")]
    [InverseProperty("UserChapters")]
    public virtual UserManga UserManga { get; set; } = null!;
}
