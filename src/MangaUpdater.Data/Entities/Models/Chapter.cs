using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

[Index("MangaId", "SourceId", "Number", Name="IX_Chapters_MangaId_SourceId_Number")]
public sealed class Chapter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public required int MangaId { get; set; }
    
    public required int SourceId { get; set; }
    
    [Column(TypeName = "datetime2")]
    public required DateTime Date { get; set; }
    
    [StringLength(450)]
    [Unicode(false)]
    public required string Number { get; set; }

    [ForeignKey("MangaId")]
    [InverseProperty("ChapterId")]
    [JsonIgnore] 
    public Manga? Manga { get; set; }
    
    [ForeignKey("SourceId")]
    [InverseProperty("ChapterId")]
    [JsonIgnore] 
    public Source? Source { get; set; }
    
    [JsonIgnore] 
    public IEnumerable<UserChapter>? UserChapter { get; set; }
}