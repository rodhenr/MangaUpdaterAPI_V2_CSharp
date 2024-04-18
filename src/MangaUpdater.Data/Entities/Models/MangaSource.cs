using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public sealed class MangaSource
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Key]
    public required int MangaId { get; set; }
    
    [Key]
    public required int SourceId { get; set; }
    
    [StringLength(100)]
    [Unicode(false)]
    public required string Url { get; set; }
    
    [ForeignKey("MangaId")]
    [InverseProperty("MangaSources")]
    [JsonIgnore] 
    public Manga? Manga { get; set; }
    
    [ForeignKey("SourceId")]
    [InverseProperty("MangaSources")]
    [JsonIgnore] 
    public Source? Source { get; set; }
}