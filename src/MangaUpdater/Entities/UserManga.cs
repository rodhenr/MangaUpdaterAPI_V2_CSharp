using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Infrastructure.Entities;

[Index("MangaId", "UserId", Name = "IX_UserMangas_MangaId_UserId", IsUnique = true)]
public partial class UserManga
{
    public string UserId { get; set; } = null!;

    public int MangaId { get; set; }

    [Key]
    public int Id { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("UserMangas")]
    public virtual Manga Manga { get; set; } = null!;

    [InverseProperty("UserManga")]
    public virtual ICollection<UserChapter> UserChapters { get; set; } = new List<UserChapter>();
}
