using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace MangaUpdater.Data.Entities.Models;

public sealed class MangaAuthor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Key]
    public required int MangaId { get; set; }
    
    [Key]
    [StringLength(450)]
    [Unicode(false)]
    public required string Name { get; set; }
    
    [ForeignKey("MangaId")]
    [JsonIgnore] 
    public Manga? Manga { get; set; }
}