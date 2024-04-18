using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

[Index("MangaId", "IsMainTitle", Name="IX_MangaTitles_MangaId_IsMainTitle")]
public sealed class MangaTitle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public required int MangaId { get; set; }
    
    [Column(TypeName = "nvarchar(MAX)")]
    [StringLength(int.MaxValue)]
    [Unicode(false)]
    public required string Name { get; set; }
    
    [DefaultValue(0)]
    public bool IsMainTitle { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("MangaTitles")]
    [JsonIgnore] 
    public Manga? Manga { get; set; }
}