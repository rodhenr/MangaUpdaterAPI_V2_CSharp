using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

[Index("MangaId", "UserId", Name="IX_UserMangas_MangaId_UserId")]
public sealed class UserManga
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [StringLength(450)]
    [Unicode(false)]
    public required string UserId { get; set; }
    
    public required int MangaId { get; set; }
    
    [ForeignKey("MangaId")]
    [InverseProperty("UserMangas")]
    [JsonIgnore]
    public Manga Manga { get; set; }

    [InverseProperty("UserManga")]
    [JsonIgnore]
    public List<UserChapter> UserChapter { get; set; } = [];
}