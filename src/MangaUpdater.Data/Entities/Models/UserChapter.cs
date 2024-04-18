using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MangaUpdater.Data.Entities.Models;

public sealed class UserChapter
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public required int UserMangaId { get; set; }
    
    public required int SourceId { get; set; }
    
    [DefaultValue(0)]
    public int? ChapterId { get; set; }
    
    [ForeignKey("Id")]
    [InverseProperty("UserChapter")]
    [JsonIgnore] 
    public UserManga? UserManga { get; set; }
    
    [ForeignKey("Id")]
    [InverseProperty("UserChapter")]
    [JsonIgnore] 
    public Source? Source { get; set; }
    
    [ForeignKey("Id")]
    [InverseProperty("UserChapter")]
    [JsonIgnore] 
    public Chapter? Chapter { get; set; }
}